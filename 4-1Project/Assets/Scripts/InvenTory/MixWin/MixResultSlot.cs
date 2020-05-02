using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MixResultSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Image UI_item_image;
    //private Text UI_item_count;

    public Item item;

    private void Awake()
    {
        UI_item_image = GetComponent<Image>();
        // UI_item_count = GetComponentInChildren<Text>();
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
        // UI_item_count.text = "";
        SetAlpha(0);
        item.itemID = 0;
        item.itemName = "";
        item.itemIcon = null;
        item.itemDescription = "";
        item.itemCount = 0;
    }

    public void InitUI()
    {
        UI_item_image.sprite = item.itemIcon;
        SetAlpha(1);
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
        InventorySlot inventorySlot = eventData.pointerEnter.gameObject.GetComponent<InventorySlot>();
        if(inventorySlot != null)
        {
            bool isGet = false;
            // 장비창이 열려있고 장비아이템을 드롭
            if (Inventory.instance._tabIndex == 0 && item.itemID > 200)
            {
                Inventory.instance.InputEquipItem(item);
                isGet = true;
            }
            // 재료창이 열려있고 재료아이템을 드롭
            else if (Inventory.instance._tabIndex == 1 && item.itemID > 0 && item.itemID <= 100)
            {
                Inventory.instance.InputMatItem(item);
                isGet = true;
            }
            else if (Inventory.instance._tabIndex == 2 && item.itemID > 100 && item.itemID <= 200) // 소비창이 열려있고 소비아이템을 드롭
            {
                Inventory.instance.InputPotionItem(item);
                isGet = true;
            }
            else { }

            if (!isGet)
            {
                DragSlot.instance.SetColor(0);
                return;
            }
            
            SetAlpha(0);
            RemoveItem(); // 조합결과 슬롯 삭제
            MixWindow.instance.selectSlotNum = 0;
        }
        DragSlot.instance.SetColor(0);
    }

}
