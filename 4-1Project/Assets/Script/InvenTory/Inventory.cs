﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    private DataBase theDataBase; // 아이템, 조합 관련 데이터베이스 받아오기

    private InventorySlot[] inventorySlots; // 인벤토리 슬롯
    private MixMaterialSlot[] mixMaterialSlots; // 조합 슬롯
    private MixResultSlot mixResultSlot; // 조합결과 슬롯

    private int[] mat_itemID; // 조합 재료 아이템 번호를 정렬하기 위한 배열
    private int[] mat_itemCount; // 조합 재료 아이템 갯수를 정렬하기 위한 배열
    private bool isMixed; // 조합 성공 여부를 서버에서 받아옴
    private int isMixItemID; // 조합 성공 시 서버에서 받아오는 아이템 번호

    public Transform inv_slot, mix_slot;

    public ItemMix Data;

    private void Awake()
    {
        instance = this;
        inventorySlots = inv_slot.GetComponentsInChildren<InventorySlot>();
        mixMaterialSlots = mix_slot.GetComponentsInChildren<MixMaterialSlot>();
        mixResultSlot = mix_slot.GetComponentInChildren<MixResultSlot>();
        theDataBase = FindObjectOfType<DataBase>();

        mat_itemID = new int[mixMaterialSlots.Length];
        mat_itemCount = new int[mixMaterialSlots.Length];

        isMixed = false;
        isMixItemID = 0;
    }

    public bool GetItem(int _itemID) // 인게임 필드에서 새로운 아이템을 획득할 경우에 호출하는 함수.
    {
        for (int i = 0; i < theDataBase.itemList.Count; i++) // 아이템 데이터베이스
        {
            if(theDataBase.itemList[i].itemID == _itemID) // 데이터베이스 아이템 ID == 습득한 아이템 ID
            {
                for (int j = 0; j < inventorySlots.Length; j++) // 인벤토리에 같은 ID의 아이템을 보유하고 있는지를 판단.
                {
                    if (inventorySlots[j].item.itemID == _itemID) // 같은 ID의 아이템을 보유하고 있으면
                    {
                        inventorySlots[j].PlusItemCount();
                        return true;
                    }
                }

                for (int j = 0; j < inventorySlots.Length; j++) // 인벤토리 슬롯 for문
                {
                    if (inventorySlots[j].item.itemID == 0) // 아이템이 비어있는 슬롯을 찾으면
                    {
                        inventorySlots[j].AddItem(theDataBase.itemList[i]); // 비어있는 슬롯에 아이템을 넣어줌
                        return true;
                    }
                }
            }
        }
        return false;
    }
    
    // 타 슬롯에서 인벤토리 슬롯으로 아이템이 넘어올 때, 슬롯에 같은 ID의 아이템 유무를 판별함.
    public InventorySlot SearchInventorySlot(int _itemID) 
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].item.itemID == _itemID)
                return inventorySlots[i];
        }
        return null;
    }

    // 타 슬롯에서 조합 슬롯으로 아이템이 넘어올 때, 슬롯에 같은 ID의 아이템 유무를 판별함.
    public MixMaterialSlot SearchMixMaterialSlot(int _itemID)
    {
        for(int i=0;i<mixMaterialSlots.Length;i++)
        {
            if (mixMaterialSlots[i].item.itemID == _itemID)
                return mixMaterialSlots[i];
        }
        return null;
    }

    public void CheckMaterial() // 조합 판단
    {
        for (int i = 0; i < 3; i++) // 조합 슬롯 3개의 itemID랑 itemCount를 받아온다.
        {
            Debug.Log("아이템 정보 대입중");
            mat_itemID[i] = mixMaterialSlots[i].item.itemID;
            mat_itemCount[i] = mixMaterialSlots[i].item.itemCount;
        }
        Debug.Log(mat_itemID[0] + "," + mat_itemID[1] + "," + mat_itemID[2]);
        Debug.Log(mat_itemCount[0] + "," + mat_itemCount[1] + "," + mat_itemCount[2]);

        for (int i = 0; i < 2; i++) // 조합 슬롯 3개의 ItemID를 오름차순으로 정렬한다.(itemCount도 같이 변경)
        {
            for (int j = i + 1; j < 3; j++)
            {
                if (mat_itemID[i] > mat_itemID[j])
                {
                    int temp = mat_itemID[i];
                    mat_itemID[i] = mat_itemID[j];
                    mat_itemID[j] = temp;

                    temp = mat_itemCount[i];
                    mat_itemCount[i] = mat_itemCount[j];
                    mat_itemCount[j] = temp;
                }
                Debug.Log("아이템 정렬 중");
            }
        }

        // 서버로 조합 슬롯의 데이터 전송
        Data.Init(mat_itemID, mat_itemCount, 10000); 
        JsonData SendData = JsonMapper.ToJson(Data);
        ServerClient.instance.Send(SendData.ToString()); // Send와 동시에 Resolve받아 조합 성공 여부를 알려줌.
    }

    private void Update()
    {
        if (isMixed) // 조합 성공 시
        {
            for (int i = 0; i < 3; i++)
            {
                Debug.Log("아이템 삭제 완료");
                mixMaterialSlots[i].RemoveItem(); // 조합 슬롯의 아이템을 모두 없앤 다음
            }

            for (int i = 0; i < theDataBase.itemList.Count; i++) // 아이템 데이터베이스에서 ID에 맞는 아이템을 찾은 뒤
            {
                if (isMixItemID == theDataBase.itemList[i].itemID)
                {
                    mixResultSlot.item.itemIcon = theDataBase.itemList[i].itemIcon; // 아이콘 삽입
                    mixResultSlot.item = theDataBase.itemList[i].Init(); // 아이템 정보를 조합결과 슬롯에 넣어준다.
                    mixResultSlot.item.itemCount = 1;
                    mixResultSlot.InitUI();
                }
            }
            isMixed = false;
            isMixItemID = 0;
        }
    }

    public void ReceiveMixResult(JsonData _data)
    {
        try
        {
            isMixed = bool.Parse(_data["result"].ToString());
            if(isMixed)
                isMixItemID = int.Parse(_data["Item"].ToString());
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
}
