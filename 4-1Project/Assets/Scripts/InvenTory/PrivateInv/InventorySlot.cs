using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using LitJson;
using System;

public class InventorySlot : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    private Image UI_item_image;
    private Text UI_item_count;
    
    public Item item;

    public SendShareInvInfo Data;

    private void Awake()
    {
        UI_item_image = GetComponent<Image>();
        UI_item_count = GetComponentInChildren<Text>();
    }

    private void OnDisable()
    {
        ItemTooltip.instance.gameObject.SetActive(false);
    }

    private void SetAlpha(float _alpha)
    {
        Color color = UI_item_image.color;
        color.a = _alpha;
        UI_item_image.color = color;
    }

    public void AddItem(Item item) // 빈 슬롯에 아이템의 정보를 삽입하는 과정
    {
        this.item = item.Init();
        InitUI();
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
        if (item.itemCount == 0)
            RemoveItem();
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
        if (item.itemID > 200) // 장비아이템일 경우 아이템카운트를 표시하지않는다.
            UI_item_count.text = "";
        else // 재료 / 소비아이템일 경우 아이템카운트를 표시한다.
            UI_item_count.text = item.itemCount.ToString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(item.itemID != 0) // 아이템이 있어야 드래그 가능
        {
            DragSlot.instance.DragSetImage(UI_item_image.sprite);
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        DragSlot.instance.transform.position = eventData.position;
    }

    public void OnDrop(PointerEventData eventData) // 그냥 드래그가 끝나고 드롭될 경우
    {
    }

    public void OnEndDrag(PointerEventData eventData) // 다른 슬롯 위에서 드롭되었을 경우
    {
        try
        {
            MixMaterialSlot mixMaterialSlot = eventData.pointerEnter.gameObject.GetComponent<MixMaterialSlot>();
            if (mixMaterialSlot != null) // 드롭한 슬롯이 조합재료 슬롯일 경우
            {
                if(item.itemID > 100) // 소비 / 장비 아이템이 드롭되면
                    throw new Exception();
                
                if(mixMaterialSlot.item.itemID == item.itemID)
                {
                    mixMaterialSlot.PlusItemCount();
                    MinusItemCount();
                }

                InitUI();

                if (item.itemCount == 0)
                    RemoveItem();
                
                Inventory.instance.UpdateItemArray(2); // 재료 슬롯 업데이트
            }

            ShareInventorySlot shareInventorySlot = eventData.pointerEnter.gameObject.GetComponent<ShareInventorySlot>();
            if (shareInventorySlot != null) // 드롭한 슬롯이 공유인벤토리일 경우
            {
                Data.Init(shareInventorySlot.slotIndex,item.itemID);
                JsonData SendData = JsonMapper.ToJson(Data);
                ServerClient.instance.Send(SendData.ToString());

                ShareInventorySlot temp = Inventory.instance.SearchShareInventorySlot(item.itemID); // 공유 슬롯에 같은 ID의 아이템이 있는지 검색
                if(item.itemID > 200) // 드래그한 아이템이 장비아이템일경우
                {
                    if (shareInventorySlot.item.itemID == 0) // 드롭한 슬롯이 빈 슬롯일 경우
                    {
                        shareInventorySlot.item = item.Init();
                        MinusItemCount();
                        if (item.itemCount == 0)
                            RemoveItem();
                    }
                    else // 드롭한 슬롯이 빈 슬롯이 아니면
                    {
                        if (shareInventorySlot.item.itemID > 200) // 장비 아이템끼리만 스왑할 수 있음
                        {
                            Item temp2 = shareInventorySlot.item.Init();
                            shareInventorySlot.item = item.Init();
                            item = temp2.Init();
                        }
                    }

                    Inventory.instance.UpdateItemArray(0); // 장비 슬롯 업데이트
                }
                else if(item.itemID > 100) // 드래그한 아이템이 소비아이템일 경우
                {
                    if (temp != null) // 공유 슬롯에 같은 ID의 아이템이 있으면
                    {
                        MinusItemCount(); // 인벤토리 슬롯 갯수 1개 감소
                        if (item.itemCount == 0)
                            RemoveItem();
                    }
                    else if (shareInventorySlot.item.itemID == 0) // 드롭한 슬롯이 빈 슬롯일 경우
                    {
                        shareInventorySlot.item = item.Init();
                        MinusItemCount();
                        if (item.itemCount == 0)
                            RemoveItem();
                    }
                    else// 드롭한 슬롯이 빈 슬롯이 아니면
                    {
                        if (shareInventorySlot.item.itemID > 100) // 소비 아이템끼리만 스왑할 수 있음
                        {
                            Item temp2 = shareInventorySlot.item.Init();
                            shareInventorySlot.item = item.Init();
                            item = temp2.Init();
                        }
                    }
                    Inventory.instance.UpdateItemArray(1); // 소비 슬롯 업데이트
                }
                else // 드래그한 아이템이 재료아이템일경우
                {
                    if (temp != null) // 공유 슬롯에 같은 ID의 아이템이 있으면
                    {
                        MinusItemCount(); // 인벤토리 슬롯 갯수 1개 감소
                        if (item.itemCount == 0)
                            RemoveItem();
                    }
                    else if (shareInventorySlot.item.itemID == 0) // 드롭한 슬롯이 빈 슬롯일 경우
                    {
                        shareInventorySlot.item = item.Init();
                        MinusItemCount();
                        if (item.itemCount == 0)
                            RemoveItem();
                    }
                    else// 드롭한 슬롯이 빈 슬롯이 아니면
                    {
                        if (shareInventorySlot.item.itemID > 0) // 재료 아이템끼리만 스왑할 수 있음
                        {
                            Item temp2 = shareInventorySlot.item.Init();
                            shareInventorySlot.item = item.Init();
                            item = temp2.Init();
                        }
                    }
                    Inventory.instance.UpdateItemArray(2); // 재료 슬롯 업데이트
                }

                InitUI();
            }

            EquipSlot equipSlot = eventData.pointerEnter.gameObject.GetComponent<EquipSlot>();
            if (equipSlot != null)
            {
                if (item.itemID > 200) // 드래그한 아이템이 장비아이템일경우
                {
                    if (equipSlot.item.itemID == 0) // 빈 슬롯일 경우
                    {
                        equipSlot.item = item.Init();
                        MinusItemCount();
                        if (item.itemCount == 0)
                            RemoveItem();
                    }
                    else // 빈 슬롯이 아닐 경우
                    {
                        equipSlot.UpdatePlayerStat(false); // 아이템이 바뀌기 전 기존 아이템의 스탯을 빼준다
                        Item temp2 = equipSlot.item.Init();
                        equipSlot.item = item.Init();
                        item = temp2.Init();
                    }
                    equipSlot.UpdatePlayerStat(true); // 새로운 장비 아이템 스탯 업그레이드
                    Inventory.instance.UpdateItemArray(0); // 장비 슬롯 업데이트
                }
            }
            InitUI();

            DragSlot.instance.SetColor(0);
        }
        catch(Exception)
        {
            DragSlot.instance.SetColor(0);
            return;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item.itemID == 0)
            return;
        ItemTooltip.instance.ShowItemInfo(item.itemName, item.itemDescription);
        ItemTooltip.instance.transform.position = transform.position;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemTooltip.instance.gameObject.SetActive(false);
    }
}
