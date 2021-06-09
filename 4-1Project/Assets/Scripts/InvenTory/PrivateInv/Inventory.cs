using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    private Item[] equip_Inv;
    private Item[] mat_Inv;
    private Item[] potion_Inv;
    public int mymoney;

    private InventorySlot[] inventorySlots; // 인벤토리 슬롯
    private ShareInventorySlot[] shareInventorySlot; // 공유 인벤토리 슬롯

    [HideInInspector]
    public int _tabIndex; // 장비 / 재료 / 소비탭 인덱스 번호

    public Transform inv_slot, mix_slot, share_slot;
    public Text text_money;
    public InventoryTab[] inventoryTabs;
    public GameObject[] openInventory;
    
    private void Awake()
    {
        instance = this;

        // 슬롯 할당
        inventorySlots = inv_slot.GetComponentsInChildren<InventorySlot>();
        shareInventorySlot = share_slot.GetComponentsInChildren<ShareInventorySlot>();

        // 장비, 재료, 소비 아이템을 저장하는 배열 할당
        equip_Inv = new Item[inventorySlots.Length];
        mat_Inv = new Item[inventorySlots.Length];
        potion_Inv = new Item[inventorySlots.Length];

        for (int i = 0; i < equip_Inv.Length; i++)
        {
            equip_Inv[i] = new Item();
            mat_Inv[i] = new Item();
            potion_Inv[i] = new Item();
        }
        text_money.text = mymoney.ToString();

        // 서버와의 연동을 위해 공유 인벤토리의 슬롯에 따로 슬롯 인덱스를 할당해줌.
        for (int i = 0; i < shareInventorySlot.Length; i++)
            shareInventorySlot[i].slotIndex = i;
    }

    private void Start()
    {
        InitEquipInv(0);
        for (int i = 0; i < openInventory.Length; i++)
            openInventory[i].SetActive(false);
    }

    private void Update()
    {
        for (int i = 0; i < shareInventorySlot.Length; i++)
            shareInventorySlot[i].InitUI();
    }

    public void UpdateItemArray(int _tabIndex)
    {
        switch (_tabIndex)
        {
            case 0: // 장비
                UpdateInvArray(equip_Inv);
                break;
            case 1: // 소비
                UpdateInvArray(potion_Inv);
                break;
            case 2: // 재료
                UpdateInvArray(mat_Inv);
                break;
        }

    }

    private void UpdateInvArray(Item[] invArray)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
            invArray[i] = inventorySlots[i].item.Init();
    }

    public void InputEquipItem(Item _item)
    {
        for(int i=0;i<equip_Inv.Length;i++)
        {
            if (equip_Inv[i].itemID == 0)
            {
                equip_Inv[i] = _item.Init();
                equip_Inv[i].itemCount = 1;
                inventorySlots[i].item = equip_Inv[i].Init();
                inventorySlots[i].InitUI();
                return;
            }
        }
    }

    public void InputMatItem(Item _item)
    {
        for (int i = 0; i < mat_Inv.Length; i++)
        {
            if (mat_Inv[i].itemID == _item.itemID)
            {
                mat_Inv[i].itemCount++;
                inventorySlots[i].item = mat_Inv[i].Init();
                inventorySlots[i].InitUI();
                return;
            }
        }
        for (int i = 0; i < mat_Inv.Length; i++)
        {
            if (mat_Inv[i].itemID == 0)
            {
                mat_Inv[i] = _item.Init();
                mat_Inv[i].itemCount = 1;
                inventorySlots[i].item = mat_Inv[i].Init();
                inventorySlots[i].InitUI();
                return;
            }
        }
    }

    public void InputPotionItem(Item _item)
    {
        for (int i = 0; i < potion_Inv.Length; i++)
        {
            if (potion_Inv[i].itemID == _item.itemID)  // 인벤토리에 같은 아이템이 있는지 확인한다.
            {
                potion_Inv[i].itemCount++; // 같은 아이템이 있으면 갯수를 올려준다
                inventorySlots[i].item = potion_Inv[i].Init();
                inventorySlots[i].InitUI();
                return;
            }
        }
        for (int i = 0; i < potion_Inv.Length; i++) // 인벤토리에 같은 아이템이 없으면 다시 검색해서 가장 먼저 비어있는 슬롯에 이에팀을 넣어준다.
        {
            if (potion_Inv[i].itemID == 0)
            {
                potion_Inv[i] = _item.Init();
                potion_Inv[i].itemCount = 1;
                inventorySlots[i].item = potion_Inv[i].Init();
                inventorySlots[i].InitUI();
                return;
            }
        }
    }

    private bool CheckEmptyItem(Item[] _item, int _DBindex, int _openTap) // 아이템배열, 아이템 번호, 현재 열린 탭 정보
    {
        for (int i = 0; i < _item.Length; i++) // 아이템 저장 배열 내 검색
        {
            if (_item[i].itemID == 0) // 빈 슬롯을 찾으면
            {
                _item[i] = DataBase.instance.itemList[_DBindex].Init();
                if (_tabIndex == _openTap) // 현재 열린 창이 아이템 저장 배열과 일치하면
                {
                    inventorySlots[i].item = equip_Inv[i]; //  UI 즉시 반영
                    inventorySlots[i].InitUI();
                }
                return true;
            }
        }
        return false;
    }
    private bool CheckEqualItem(Item[] _item, int _equalItemID, int _openTap)
    {
        for (int i = 0; i < _item.Length; i++) // 같은 ID의 아이템이 있는지 체크
        {
            if (_item[i].itemID == _equalItemID) // 슬롯에 같은 아이템이 있으면
            {
                _item[i].itemCount++; // 아이템 갯수 1 증가
                if (_tabIndex == _openTap) // 현재 열린 창이 아이템 저장 배열과 일치하면
                {
                    inventorySlots[i].item = _item[i]; //  UI 즉시 반영
                    inventorySlots[i].InitUI();
                }
                return true;
            }
        }
        return false;
    }
    public bool GetItem(int _itemID) // 인게임 필드에서 새로운 아이템을 획득할 경우에 호출하는 함수.
    {
        for (int i = 0; i < DataBase.instance.itemList.Count; i++) // 아이템 데이터베이스
        {
            if (DataBase.instance.itemList[i].itemID == _itemID) // 데이터베이스 아이템 ID == 습득한 아이템 ID
            {
                if (_itemID > 200) // 장비템 습득
                {
                    return CheckEmptyItem(equip_Inv, i, 0);
                }
                else if (_itemID > 100) // 소비템 습득
                {
                    if (!CheckEqualItem(potion_Inv, i, 2)) // 같은 아이템이 없으면 빈 슬롯에 아이템을 넣어준다.
                        return CheckEmptyItem(potion_Inv, i, 2);
                    else
                        return true;
                }
                else // 재료템 습득
                {
                    if (!CheckEqualItem(mat_Inv, i, 1)) // 같은 아이템이 없으면 빈 슬롯에 아이템을 넣어준다.
                        return CheckEmptyItem(mat_Inv, i, 1);
                    else
                        return true;
                }
            }
        }
        return false;
    }

    public void Backup(int _itemID, int _itemCount)
    {
        // 재료 인벤토리를 검색한다(같은 아이템이 있을 경우 이 for문에서 끝)
        for (int i = 0; i < mat_Inv.Length; i++)
        {
            // 재료 인벤토리의 아이템과 반환하려는 아이템ID가 일치하면
            if (mat_Inv[i].itemID == _itemID)
            {
                // 그 재료 아이템의 갯수를 올려준 후
                mat_Inv[i].itemCount += _itemCount;

                if (_tabIndex == 1) // 재료 탭이 열려있을 경우
                    inventorySlots[i].InitUI(); // 인벤토리를 갱신해준다.
                return;
            }
        }
        // 재료 인벤토리 검색(같은 아이템이 없을 경우)
        for (int i = 0; i < mat_Inv.Length; i++)
        {
            if(mat_Inv[i].itemID == 0) // 빈 공간이 있으면
            {
                for(int j=0;i<DataBase.instance.itemList.Count;j++) // DB검색
                {
                    if (DataBase.instance.itemList[j].itemID == _itemID) // 아이템 찾아
                    {
                        mat_Inv[i] = DataBase.instance.itemList[j].Init(); // 그 슬롯에 생성.

                        if (_tabIndex == 1) // 재료 탭이 열려있을 경우
                        {
                            inventorySlots[i].item = mat_Inv[i].Init(); // 인벤토리를 갱신해준다.
                            inventorySlots[i].InitUI();
                        }
                        return;
                    }
                }
            }
        }
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

    // 타 슬롯에서 공유 슬롯으로 아이템이 넘어올 때, 슬롯에 같은 ID의 아이템 유무를 판별함.
    public ShareInventorySlot SearchShareInventorySlot(int _itemID)
    {
        for (int i = 0; i < shareInventorySlot.Length; i++)
        {
            if (shareInventorySlot[i].item.itemID == _itemID)
                return shareInventorySlot[i];
        }
        return null;
    }

    public void UpdateShareInfo(JsonData _data) // 공유 인벤토리 갱신
    {
        try
        {
            for (int i = 0; i < shareInventorySlot.Length; i++)
            {
                shareInventorySlot[i].item.itemID = int.Parse(_data["Inventory"][i].ToString());
                shareInventorySlot[i].item.itemCount = int.Parse(_data["ItemCount"][i].ToString());
                for (int j = 0; j < DataBase.instance.itemList.Count; j++)
                {
                    if (shareInventorySlot[i].item.itemID == DataBase.instance.itemList[j].itemID)
                    {
                        shareInventorySlot[i].item.itemIcon = DataBase.instance.itemList[j].itemIcon;
                        shareInventorySlot[i].item.itemDescription = DataBase.instance.itemList[j].itemDescription;
                        shareInventorySlot[i].item.itemName = DataBase.instance.itemList[j].itemName;
                    }
                }
            }
        }
        catch (Exception)
        {
            return;
        }
    }

    void UpdateItemInfo(int _tabIndex) // 탭을 바꿀 때마다 바꾸기 전 탭의 정보를 아이템 배열에 저장
    {
        switch (_tabIndex)
        {
            case 0: // 장비
                for (int i = 0; i < inventorySlots.Length; i++)
                    equip_Inv[i] = inventorySlots[i].item.Init();
                break;
            case 1: // 재료
                for (int i = 0; i < inventorySlots.Length; i++)
                    mat_Inv[i] = inventorySlots[i].item.Init();
                break;
            case 2: // 소비
                for (int i = 0; i < inventorySlots.Length; i++)
                    potion_Inv[i] = inventorySlots[i].item.Init();
                break;
        }
    }

    void ChangeTabColor(int _p_tabNum)
    {
        inventoryTabs[_tabIndex].DisableTab(); // 이전에 선택된 인덱스의 탭 색깔을 비선택 색깔로 바꿔준 뒤
        _tabIndex = _p_tabNum; // 탭 인덱스를 바꾸고
        inventoryTabs[_tabIndex].EnableTab(); // 새로 선택된 인덱스의 탭 색깔로 바꿔준다.
    }

    void UpdateInvSlot(Item[] _items)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].item = _items[i];
            inventorySlots[i].InitUI();
        }
    }

    public bool CheckHaveKey() // 보스 방에 들어가기 위한 열쇠를 가지고 있는지를 판단
    {
        for(int i=0;i< equip_Inv.Length;i++)
        {
            if (equip_Inv[i].itemID == 207) // 키 가지고 있으면
                return true; 
        }
        return false;
    }

    #region ClickTab
    public void InitEquipInv(int _tabNum) // 장비 탭 클릭 시
    {
        UpdateItemInfo(_tabIndex);
        UpdateInvSlot(equip_Inv);
        ChangeTabColor(_tabNum);
    }

    public void InitMatInv(int _tabNum) // 재료 탭 클릭 시
    {
        UpdateItemInfo(_tabIndex);
        UpdateInvSlot(mat_Inv);
        ChangeTabColor(_tabNum);
    }

    public void InitPotionInv(int _tabNum) // 소비 탭 클릭 시
    {
        UpdateItemInfo(_tabIndex);
        UpdateInvSlot(potion_Inv);
        ChangeTabColor(_tabNum);
    }
    #endregion


}
