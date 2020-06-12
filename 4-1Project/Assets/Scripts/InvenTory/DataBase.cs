using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 0 : 아이템 없음
// 1 ~ 100 : 재료 (1)
// 101 ~ 200 : 소비 (2)
// 201 ~ 300 : 장비 (0)

public class DataBase : MonoBehaviour
{
    public static DataBase instance;

    public List<Item> itemList = new List<Item>();
    public List<Recipe> armorRecipeList = new List<Recipe>();
    public List<Recipe> weaponRecipeList = new List<Recipe>();
    public List<Recipe> subweaponRecipeList = new List<Recipe>();
    public List<Recipe> accessoryRecipeList = new List<Recipe>();
    public List<Recipe> activeRecipeList = new List<Recipe>();

    private void Awake()
    {
        instance = this;
    }
    // ITEM : ItemID , ItemName, ItemDes, ItemSpriteFilename
    // RECIPE : (item1ID, item1Count, item1filename), (2), (3), money, resultitemfilepath, resultitemID

    private void Start()
    {
        // Material Item
        itemList.Add(new Item(1, "헤르나이트", "헤르나이트", "hernite"));
        itemList.Add(new Item(2, "아스바타의 파편", "아스바타의 파편.", "ashvattha"));
        itemList.Add(new Item(3, "나뭇가지", "나뭇가지", "twig"));
        itemList.Add(new Item(4, "천 조각", "천 조각.", "fabric"));
        itemList.Add(new Item(5, "낡은 철조각", "낡은 철조각.", "iron"));
        itemList.Add(new Item(6, "연마제", "연마제", "powder"));
        itemList.Add(new Item(7, "가이테리움", "가이테리움", "gaiter"));
        itemList.Add(new Item(8, "에레로늄", "에레로늄.", "Ereronium"));
        itemList.Add(new Item(9, "빛바랜 훈장", "빛바랜 훈장", "pendant"));
        itemList.Add(new Item(10, "연금서", "태규형한테 받아올 코멘트", "alchemy"));

        itemList.Add(new Item(11, "낡은 종이", "1312", "PasswordPaper"));

        // Potion Item

        // Equipment Item
        itemList.Add(new Item(207, "열쇠", "이 열쇠가 있어야 보스방에 입장할 수 있습니다.", "bosskey"));
        itemList.Add(new Item(201, "선지자의 로브", "문구1", "robe"));
        itemList.Add(new Item(202, "선지자의 탈리스만", "문구1", "talisman"));
        itemList.Add(new Item(203, "선지자의 스태프", "문구1", "staff"));
        itemList.Add(new Item(204, "선구자의 갑옷", "문구1", "armor"));
        itemList.Add(new Item(205, "선구자의 깃발", "문구1", "gauntlet"));
        itemList.Add(new Item(206, "선구자의 망치", "문구1", "hammer"));


        // RECIPE : (item1ID, item1Count, item1filename), (2), (3), money, resultitemfilepath, resultitemID

        // Armor Recipe
        armorRecipeList.Add(new Recipe(4, 1, "fabric", 8, 1, "Ereronium", 6, 1, "powder", 5000, "robe", 201));
        armorRecipeList.Add(new Recipe(5, 1, "iron", 9, 1, "pendant", 6, 1, "powder", 5000, "armor", 204));

        // Weapon Recipe
        weaponRecipeList.Add(new Recipe(3, 1, "twig", 2, 1, "ashvattha", 6, 1, "powder", 5000, "staff", 203));
        weaponRecipeList.Add(new Recipe(5, 1, "iron", 1, 1, "hernite", 6, 1, "powder", 5000, "hammer", 206));
        // SubWeapon Recipe

        // Accessory Recipe
        accessoryRecipeList.Add(new Recipe(10, 1, "alchemy", 8, 1, "Ereronium", 7, 1, "gaiter", 5000, "talisman", 202));
        accessoryRecipeList.Add(new Recipe(9, 1, "pendant", 8, 1, "Ereronium", 7, 1, "gaiter", 5000, "gauntlet", 205));

        // Active Recipe
        activeRecipeList.Add(new Recipe(1, 1, "hernite", 4, 1, "fabric", 7, 1, "gaiter", 10000, "bosskey", 207));
    }
}
