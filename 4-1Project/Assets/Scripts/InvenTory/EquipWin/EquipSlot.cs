using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour, IBeginDragHandler, IDragHandler,  IEndDragHandler
{
    private Image _UI_image;

    private Player _player;
    public EquipDatabase equipDatabase;
    public Item item;
    
    private int _nowSTR;
    private int _nowDEF;

    private void Awake()
    {
        _UI_image = GetComponent<Image>();
    }

    private void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    public void UpdatePlayerStat(bool _isInstall)
    {
        if(_isInstall)
        {
            for(int i=0;i<equipDatabase.equipInfoList.Count;i++)
            {
                if(item.itemID == equipDatabase.equipInfoList[i].itemID)
                {
                    _nowSTR = equipDatabase.equipInfoList[i].STR;
                    _nowDEF = equipDatabase.equipInfoList[i].DEF;
                }
            }
            _player.STR += _nowSTR;
            _player.DEF += _nowDEF;
            _UI_image.sprite = item.itemIcon;
            _UI_image.color = new Color(1, 1, 1, 1);
        }
        else
        {
            _player.STR -= _nowSTR;
            _player.DEF -= _nowDEF;
            _UI_image.sprite = null;
            _UI_image.color = new Color(1, 1, 1, 0);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item.itemID != 0) // 아이템이 있어야 드래그 가능
        {
            DragSlot.instance.DragSetImage(item.itemIcon);
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
            InventorySlot temp = eventData.pointerEnter.gameObject.GetComponent<InventorySlot>();
            if (temp != null)
            {
                if (temp.item.itemID == 0) // 인벤토리 슬롯이 빈 공간일 경우
                {
                    temp.item = item.Init(); // 인벤토리 슬롯에 아이템 정보를 넘겨주고
                    temp.InitUI(); // UI 정보 업데이트
                    UpdatePlayerStat(false);
                    item.itemID = 0;
                    _nowSTR = 0;
                    _nowDEF = 0;
                }
                else // 다른 장비 아이템이 있을 경우
                {
                    if (temp.item.itemID > 200)
                    {
                        UpdatePlayerStat(false); // 기존 장비 스탯을 죽인 후

                        Item temp2 = temp.item.Init(); // 아이템 변경 후
                        temp.item = item.Init();
                        item = temp2.Init();

                        UpdatePlayerStat(true); // 새로운 장비 스탯을 올려준다.
                    }
                }
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
