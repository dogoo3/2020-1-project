﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MixMaterialSlot : MonoBehaviour,
    IDragHandler,IBeginDragHandler,IEndDragHandler
{
    private Image UI_item_image;
    private Text UI_item_count;

    public Item item;

    public int _maxItemCount;

    private void Awake()
    {
        UI_item_image = GetComponent<Image>();
        UI_item_count = GetComponentInChildren<Text>();
    }

    private void SetAlpha(float _alpha)
    {
        Color color = UI_item_image.color;
        color.a = _alpha;
        UI_item_image.color = color;
    }

    public void RemoveItem()
    {
        UI_item_image.sprite = null;
        UI_item_count.text = "";
        SetAlpha(0);
        item.itemID = 0;
        item.itemName = "";
        item.itemIcon = null;
        item.itemDescription = "";
        item.itemCount = 0;
    }

    public void PlusItemCount()
    {
        item.itemCount++;
        InitUI();
    }

    public void MinusItemCount()
    {
        item.itemCount--;
        InitUI();
    }

    public void SetSlot(Sprite matIcon, int itemID, int itemCount)
    {
        for(int i=0;i<DataBase.instance.itemList.Count;i++)
        {
            if (DataBase.instance.itemList[i].itemID == itemID)
            {
                item = DataBase.instance.itemList[i].Init();
                break;
            }
        }
        item.itemID = itemID;
        item.itemCount = 0;
        _maxItemCount = itemCount;
        UI_item_image.sprite = matIcon;
        SetAlpha(1);
        InitUI();
    }

    // 아이템 반환 함수
    public void BackupItem()
    {
        if (item.itemCount == 0) // 제작창에 넣은 아이템의 갯수가 0이면
            return; // 아이템을 반환하지 않는다.
        Inventory.instance.Backup(item.itemID, item.itemCount);
    }

    public void InitUI()
    {
        // UI_item_image.sprite = item.itemIcon;
        // SetAlpha(1);
        UI_item_count.text = item.itemCount.ToString() + " / " + _maxItemCount.ToString();
        // if (item.itemCount == 0)
        //    RemoveItem();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item.itemID != 0 && item.itemCount > 0) // 아이템이 있어야 드래그 가능
        {
            DragSlot.instance.DragSetImage(UI_item_image.sprite);
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        DragSlot.instance.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        try
        {
            InventorySlot inventorySlot = eventData.pointerEnter.gameObject.GetComponent<InventorySlot>();
            if (inventorySlot != null) // 드롭한 슬롯이 인벤토리 슬롯일 경우
            {
                InventorySlot temp = Inventory.instance.SearchInventorySlot(item.itemID); // 인벤토리 슬롯에 같은 ID의 아이템이 있는지 검색
                if (temp != null) // 인벤토리 슬롯에 같은 아이템이 있으면
                {
                    temp.PlusItemCount(); // 인벤토리 슬롯의 갯수 1개 증가
                    MinusItemCount(); // 조합 슬롯 갯수 1개 감소
                    temp.InitUI(); // 인벤토리 슬롯 UI 업데이트
                }
                else // 인벤토리 슬롯에 같은 ID의 아이템이 없을 경우
                {
                    // 드롭한 슬롯의 아이템 ID가 0이면
                    if (inventorySlot.item.itemID == 0)
                    {
                        inventorySlot.item = item.Init(); // 인벤토리 슬롯에 아이템 정보 할당
                        inventorySlot.item.itemCount = 1; // 아이템의 갯수는 1로 초기화
                        MinusItemCount(); // 조합 슬롯 갯수 1개 감소
                    }
                    //// 0이 아니면 아무것도 하지 않는다.
                    //else
                    //{
                    //    Item temp2 = inventorySlot.item.Init();
                    //    inventorySlot.item = item.Init();
                    //    item = temp2.Init();
                    //}
                }

                InitUI();
                inventorySlot.InitUI(); // 인벤토리 슬롯 UI 업데이트

                //if (item.itemCount == 0)
                //    RemoveItem();
            }
            DragSlot.instance.SetColor(0);
        }
        catch(Exception)
        {
            DragSlot.instance.SetColor(0);
            return;
        }
    }
}
