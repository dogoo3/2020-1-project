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

    public RecipeSlot[] recipeSlots; // ������ ����
    private MixMaterialSlot[] mixMaterialSlots; // ���� ����
    private MixResultSlot mixResultSlot; // ���հ�� ����

    private int[] mat_itemID; // ���� ��� ������ ��ȣ�� �����ϱ� ���� �迭
    private int[] mat_itemCount; // ���� ��� ������ ������ �����ϱ� ���� �迭

    private bool isMixed; // ���� ���� ���θ� �������� �޾ƿ�
    private int isMixItemID; // ���� ���� �� �������� �޾ƿ��� ������ ��ȣ

    public ItemMix Data;

    public MixWindowTab[] mixWindowTabs;
    public Text text_money;
    public int selectSlotNum; // ������ ������ Ŭ������ ���� ��ȣ. ���հ�� ������ ��ȣ�� ���´�.

    [HideInInspector]
    public int _tabIndex; // ��� / ��� / �Һ��� �ε��� ��ȣ

    [Header("���� ������ ���� ����")]
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
        ClickArmorTab(); // �⺻ ������ ����
        weaponRecipe = DataBase.instance.weaponRecipeList;
        accessoryRecipe = DataBase.instance.accessoryRecipeList;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isMixed) // ���� ���� ��
        {
            for (int i = 0; i < 3; i++)
                mixMaterialSlots[i].RemoveItem(); // ���� ������ �������� ��� ���� ����

            for (int i = 0; i < DataBase.instance.itemList.Count; i++) // ������ �����ͺ��̽����� ID�� �´� �������� ã�� ��
            {
                if (isMixItemID == DataBase.instance.itemList[i].itemID)
                {
                    mixResultSlot.item.itemIcon = DataBase.instance.itemList[i].itemIcon; // ������ ����
                    mixResultSlot.item = DataBase.instance.itemList[i].Init(); // ������ ������ ���հ�� ���Կ� �־��ش�.
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

    // Ÿ ���Կ��� ���� �������� �������� �Ѿ�� ��, ���Կ� ���� ID�� ������ ������ �Ǻ���.
    public MixMaterialSlot SearchMixMaterialSlot(int _itemID)
    {
        for (int i = 0; i < mixMaterialSlots.Length; i++)
        {
            if (mixMaterialSlots[i].item.itemID == _itemID)
                return mixMaterialSlots[i];
        }
        return null;
    }

    public void CheckMaterial() // ���� �Ǵ�
    {
        for (int i = 0; i < 3; i++) // ���� ���� 3���� itemID�� itemCount�� �޾ƿ´�.
        {
            mat_itemID[i] = mixMaterialSlots[i].item.itemID;
            mat_itemCount[i] = mixMaterialSlots[i].item.itemCount;
        }

        for (int i = 0; i < 2; i++) // ���� ���� 3���� ItemID�� ������������ �����Ѵ�.(itemCount�� ���� ����)
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

        // ������ ���� ������ ������ ����
        Data.Init(mat_itemID, mat_itemCount, Inventory.instance.mymoney);
        JsonData SendData = JsonMapper.ToJson(Data);
        ServerClient.instance.Send(SendData.ToString()); // Send�� ���ÿ� Resolve�޾� ���� ���� ���θ� �˷���.
    }

    public void ReceiveMixResult(JsonData _data) // ���� ����� �������� �޾ƿ��� �Լ�
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
        mixWindowTabs[_tabIndex].DisableTab(); // ������ ���õ� �ε����� �� ������ ���� ����� �ٲ��� ��
        _tabIndex = _p_tabNum; // �� �ε����� �ٲٰ�
        mixWindowTabs[_tabIndex].EnableTab(); // ���� ���õ� �ε����� �� ����� �ٲ��ش�.
    }

    void UpdateRecipeSlot(List<Recipe> _recipes)
    {
        // ��� ������ ������ �ʱ�ȭ
        for (int i = 0; i < recipeSlots.Length; i++)
        {
            recipeSlots[i].recipe.Reset();
            recipeSlots[i].InitUI(0); // �� ���̰� ����ó��
        }
        // �Ű������� ���� ������ ������ ���Կ� �־���.
        for (int i = 0; i < _recipes.Count; i++)
        {
            recipeSlots[i].recipe = _recipes[i].Init();
            recipeSlots[i].InitUI(1); // ���̰� ���� 255
        }
        // ������ �ٸ� ���� Ŭ���ϰ� �� ��� �������� ��ȯ�����ָ鼭 ������ �ʱ�ȭ��.
        for(int i=0;i<mixMaterialSlots.Length;i++)
        {
            mixMaterialSlots[i].BackupItem();
            mixMaterialSlots[i].RemoveItem();
        }
        text_money.text = "";
        selectSlotNum = 0;
    }
    
    public void InputRecipe(int _index) // 0:�� 1:���� 2:�������� 3:�׼����� 4:��Ƽ��
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
    public void ClickArmorTab() // �� �� Ŭ�� ��
    {
        UpdateRecipeSlot(armorRecipe);
        ChangeTabColor(0);
    }

    public void ClickWeaponTab() // ���� �� Ŭ�� ��
    {
        UpdateRecipeSlot(weaponRecipe);
        ChangeTabColor(1);
    }

    public void ClickSubWeaponTab() // �������� �� Ŭ�� ��
    {
        UpdateRecipeSlot(subweaponRecipe);
        ChangeTabColor(2);
    }

    public void ClickAccessoryTab() // �׼����� �� Ŭ�� ��
    {
        UpdateRecipeSlot(accessoryRecipe);
        ChangeTabColor(3);
    }

    public void ClickActiveTab() // ��Ƽ�� �� Ŭ�� ��
    {
        UpdateRecipeSlot(activeRecipe);
        ChangeTabColor(4);
    }
    #endregion
}
