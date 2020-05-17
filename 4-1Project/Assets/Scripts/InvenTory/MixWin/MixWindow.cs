using UnityEngine;
using System.Collections;
using LitJson;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public enum MixWindowTabName
{
    Armor,
    Weapon,
    SubWeapon,
    Accessory,
    Active,
};

public class MixWindow : MonoBehaviour
{
    public static MixWindow instance;

    public RecipeSlot[] recipeSlots; // 레시피 슬롯
    private MixMaterialSlot[] mixMaterialSlots; // 조합 슬롯
    private MixResultSlot mixResultSlot; // 조합결과 슬롯

    private int[] mat_itemID; // 조합 재료 아이템 번호를 정렬하기 위한 배열
    private int[] mat_itemCount; // 조합 재료 아이템 갯수를 정렬하기 위한 배열

    private bool isMixed; // 조합 성공 여부를 서버에서 받아옴
    private int isMixItemID; // 조합 성공 시 서버에서 받아오는 아이템 번호

    public ItemMix Data;

    public MixWindowTab[] mixWindowTabs;
    public Text text_money;
    public int selectSlotNum; // 레시피 슬롯을 클릭했을 때의 번호. 조합결과 아이템 번호가 들어온다.

    [HideInInspector]
    public int _tabIndex; // 장비 / 재료 / 소비탭 인덱스 번호

    [Header("조합 아이템 정보 삽입")]
    public List<Recipe> armorRecipe;
    public List<Recipe> weaponRecipe;
    public List<Recipe> subweaponRecipe;
    public List<Recipe> accessoryRecipe;
    public List<Recipe> activeRecipe;

    private void Awake()
    {
        instance = this;

        mixMaterialSlots = GetComponentsInChildren<MixMaterialSlot>();
        mixResultSlot = GetComponentInChildren<MixResultSlot>();

        mat_itemID = new int[mixMaterialSlots.Length];
        mat_itemCount = new int[mixMaterialSlots.Length];
        
    }

    private void Start()
    {
        ClickArmorTab(); // 기본 선택은 방어구로
        weaponRecipe = DataBase.instance.weaponRecipeList;
        accessoryRecipe = DataBase.instance.accessoryRecipeList;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isMixed) // 조합 성공 시
        {
            for (int i = 0; i < 3; i++)
                mixMaterialSlots[i].RemoveItem(); // 조합 슬롯의 아이템을 모두 없앤 다음

            for (int i = 0; i < DataBase.instance.itemList.Count; i++) // 아이템 데이터베이스에서 ID에 맞는 아이템을 찾은 뒤
            {
                if (isMixItemID == DataBase.instance.itemList[i].itemID)
                {
                    mixResultSlot.item.itemIcon = DataBase.instance.itemList[i].itemIcon; // 아이콘 삽입
                    mixResultSlot.item = DataBase.instance.itemList[i].Init(); // 아이템 정보를 조합결과 슬롯에 넣어준다.
                    mixResultSlot.item.itemCount = 1;
                    mixResultSlot.InitUI();
                }
            }
            isMixed = false;
            isMixItemID = 0;
            text_money.text = "";
            Inventory.instance.text_money.text = Inventory.instance.mymoney.ToString();
        }
    }

    // 타 슬롯에서 조합 슬롯으로 아이템이 넘어올 때, 슬롯에 같은 ID의 아이템 유무를 판별함.
    public MixMaterialSlot SearchMixMaterialSlot(int _itemID)
    {
        for (int i = 0; i < mixMaterialSlots.Length; i++)
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
            mat_itemID[i] = mixMaterialSlots[i].item.itemID;
            mat_itemCount[i] = mixMaterialSlots[i].item.itemCount;
        }

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
            }
        }

        // 서버로 조합 슬롯의 데이터 전송
        Data.Init(mat_itemID, mat_itemCount, Inventory.instance.mymoney);
        JsonData SendData = JsonMapper.ToJson(Data);
        ServerClient.instance.Send(SendData.ToString()); // Send와 동시에 Resolve받아 조합 성공 여부를 알려줌.
    }

    public void ReceiveMixResult(JsonData _data) // 조합 결과를 서버에서 받아오는 함수
    {
        try
        {
            isMixed = bool.Parse(_data["result"].ToString());
            if (isMixed)
            {
                isMixItemID = int.Parse(_data["Item"].ToString());
                Inventory.instance.mymoney = int.Parse(_data["money"].ToString());
            }
        }
        catch (Exception)
        {
            return;
        }
    }

    void ChangeTabColor(int _p_tabNum)
    {
        mixWindowTabs[_tabIndex].DisableTab(); // 이전에 선택된 인덱스의 탭 색깔을 비선택 색깔로 바꿔준 뒤
        _tabIndex = _p_tabNum; // 탭 인덱스를 바꾸고
        mixWindowTabs[_tabIndex].EnableTab(); // 새로 선택된 인덱스의 탭 색깔로 바꿔준다.
    }

    void UpdateRecipeSlot(List<Recipe> _recipes)
    {
        // 모든 레시피 슬롯을 초기화
        for (int i = 0; i < recipeSlots.Length; i++)
        {
            recipeSlots[i].recipe.Reset();
            recipeSlots[i].InitUI(0); // 안 보이게 투명처리
        }
        // 매개변수로 받은 레시피 정보를 슬롯에 넣어줌.
        for (int i = 0; i < _recipes.Count; i++)
        {
            recipeSlots[i].recipe = _recipes[i].Init();
            recipeSlots[i].InitUI(1); // 보이게 투명값 255
        }
        // 제작중 다른 탭을 클릭하게 될 경우 아이템을 반환시켜주면서 슬롯을 초기화함.
        for(int i=0;i<mixMaterialSlots.Length;i++)
        {
            mixMaterialSlots[i].BackupItem();
            mixMaterialSlots[i].RemoveItem();
        }
        text_money.text = "";
        selectSlotNum = 0;
    }
    
    public void InputRecipe(int _index) // 0:방어구 1:무기 2:보조무기 3:액세서리 4:액티브
    {
        switch (_index)
        {
            case (int)MixWindowTabName.Armor:
                break;
            case (int)MixWindowTabName.Weapon:
                break;
            case (int)MixWindowTabName.SubWeapon:
                break;
            case (int)MixWindowTabName.Accessory:
                break;
            case (int)MixWindowTabName.Active:
                break;
        }
    }

    #region ClickTab
    public void ClickArmorTab() // 방어구 탭 클릭 시
    {
        UpdateRecipeSlot(armorRecipe);
        ChangeTabColor(0);
    }

    public void ClickWeaponTab() // 무기 탭 클릭 시
    {
        UpdateRecipeSlot(weaponRecipe);
        ChangeTabColor(1);
    }

    public void ClickSubWeaponTab() // 보조무기 탭 클릭 시
    {
        UpdateRecipeSlot(subweaponRecipe);
        ChangeTabColor(2);
    }

    public void ClickAccessoryTab() // 액세서리 탭 클릭 시
    {
        UpdateRecipeSlot(accessoryRecipe);
        ChangeTabColor(3);
    }

    public void ClickActiveTab() // 액티브 탭 클릭 시
    {
        UpdateRecipeSlot(activeRecipe);
        ChangeTabColor(4);
    }
    #endregion
}
