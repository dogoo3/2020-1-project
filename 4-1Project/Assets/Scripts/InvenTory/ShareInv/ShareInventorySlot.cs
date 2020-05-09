using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LitJson;
using System;

public class ShareInventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Image UI_item_image;
    private Text UI_item_count;
    
    public Item item;

    public int slotIndex; // 공유 인벤토리 슬롯 인덱스(서버와의 연동을 위해서)

    public SendShareSwapInfo swapData; // 공유 인벤토리 간 스왑 정보를 서버로 전송
    public SendShareDeleteInfo deleteData; // 공유 인벤토리 -> 개인 인벤토리 정보를 서버로 전송

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
        UI_item_count.text = item.itemCount.ToString();
    }

    public void MinusItemCount()
    {
        item.itemCount--;
        UI_item_count.text = item.itemCount.ToString();
    }

    public void InitUI()
    {
        if (item.itemCount == 0)
        {
            RemoveItem();
            return;
        }
        UI_item_image.sprite = item.itemIcon;
        SetAlpha(1);
        if (item.itemID > 200) // 아이템이 장비아이템일경우
            UI_item_count.text = "";
        else
            UI_item_count.text = item.itemCount.ToString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item.itemID != 0) // 아이템이 있어야 드래그 가능
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
            ShareInventorySlot shareInventorySlot = eventData.pointerEnter.gameObject.GetComponent<ShareInventorySlot>();
            if (shareInventorySlot != null) // 드롭한 슬롯이 공유 인벤토리 슬롯일 경우
            {
                swapData.Init(slotIndex,shareInventorySlot.slotIndex);
                JsonData SendData = JsonMapper.ToJson(swapData);
                ServerClient.instance.Send(SendData.ToString());
                
                // 슬롯 간 정보 교환
                Item temp = shareInventorySlot.item.Init();
                shareInventorySlot.item = item.Init();
                item = temp.Init();
                
                DragSlot.instance.SetColor(0);
                return;
            }

            InventorySlot inventorySlot = eventData.pointerEnter.gameObject.GetComponent<InventorySlot>();
            if (inventorySlot != null) // 드롭한 슬롯이 인벤토리 슬롯일 경우
            {
                InventorySlot temp = Inventory.instance.SearchInventorySlot(item.itemID); // 인벤토리 슬롯에 같은 ID의 아이템이 있는지 검색

                deleteData.Init(slotIndex);

                if(item.itemID > 200) // 드래그한 아이템이 장비아이템이면
                {
                    if (Inventory.instance._tabIndex != 0) // 인벤토리 장비탭이 열려있지 않은 경우
                        throw new Exception(); // 강제 에러 처리

                    if(inventorySlot.item.itemID == 0) // 드롭한 슬롯이 빈 슬롯일 경우
                    {
                        inventorySlot.item = item.Init();
                        inventorySlot.InitUI();
                        Inventory.instance.UpdateItemArray(0);
                        JsonData SendData = JsonMapper.ToJson(deleteData);
                        ServerClient.instance.Send(SendData.ToString());
                    }
                    else if(inventorySlot.item.itemID > 200) // 드롭한 슬롯이 장비 슬롯일 경우
                    {
                        Item temp2 = inventorySlot.item.Init();
                        inventorySlot.item = item.Init();
                        item = temp2.Init();
                        Inventory.instance.UpdateItemArray(0);
                        JsonData SendData = JsonMapper.ToJson(deleteData);
                        ServerClient.instance.Send(SendData.ToString());
                    }
                    else // 드롭한 슬롯이 소비/재료 슬롯일 경우
                    { }
                }
                else // 드래그한 아이템이 소비 / 재료 아이템이면
                {
                    if(item.itemID > 100 && Inventory.instance._tabIndex == 2 ||
                        item.itemID <= 100 && item.itemID > 0  && Inventory.instance._tabIndex == 1) // 아이템 타입과 맞지 않는 인벤토리 탭에 드롭하면 실행되지 않는다.
                    {
                        if (temp != null) // 인벤토리 슬롯에 같은 아이템이 있으면
                        {
                            temp.PlusItemCount(); // 인벤토리 슬롯의 갯수 1개 증가
                            temp.InitUI(); // 인벤토리 슬롯 UI 업데이트
                        }
                        else // 인벤토리 슬롯에 같은 ID의 아이템이 없을 경우
                        {
                            if (inventorySlot.item.itemID == 0)
                            {
                                inventorySlot.item = item.Init(); // 인벤토리 슬롯에 아이템 정보 할당
                                inventorySlot.item.itemCount = 1; // 아이템의 갯수는 1로 초기화
                                MinusItemCount(); // 공유 슬롯 갯수 1개 감소
                            }
                            else
                            {
                                Item temp2 = inventorySlot.item.Init();
                                inventorySlot.item = item.Init();
                                item = temp2.Init();
                            }
                        }
                        if(item.itemID > 100 && item.itemID <= 200) // 소비 아이템
                            Inventory.instance.UpdateItemArray(1);
                        else // 재료 아이템
                            Inventory.instance.UpdateItemArray(2);

                        JsonData SendData = JsonMapper.ToJson(deleteData);
                        ServerClient.instance.Send(SendData.ToString());
                    }
                }

                inventorySlot.InitUI();

                if (item.itemCount == 0)
                    RemoveItem();
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
