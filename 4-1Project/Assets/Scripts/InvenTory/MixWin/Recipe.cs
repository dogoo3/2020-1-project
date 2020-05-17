using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Recipe
{
    public Sprite icon; // 완성 아이템 아이콘
    public List<Sprite> mat_icon; // 재료 아이템 아이콘
    public int[] itemID = new int[3]; // 재료 아이템 ID
    public int[] itemCount = new int[3]; // 재료 아이템 갯수
    public int money; // 재료비
    public int slotResultnum; // 완성아이템 번호

    public Recipe()
    {
        icon = null;
        for(int i=0;i<3;i++)
        {
            itemID[i] = new int();
            itemCount[i] = new int();
        }
        money = 0;
        slotResultnum = 0;
    }

    public Recipe(int id1, int count1, string filename1, int id2, int count2, string filename2, int id3, int count3, string filename3, int money, string resultfilename, int resultItemID)
    {
        for (int i = 0; i < 3; i++)
        {
            itemID[i] = new int();
            itemCount[i] = new int();
        }
        // 재료아이콘 리스트 할당
        mat_icon = new List<Sprite>();
        // 재료아이템 ID
        itemID[0] = id1;
        itemID[1] = id2;
        itemID[2] = id3;
        // 재료아이템 갯수
        itemCount[0] = count1;
        itemCount[1] = count2;
        itemCount[2] = count3;
        // 재료비
        this.money = money;
        // 결과아이템 이미지
        icon = Resources.Load(resultfilename, typeof(Sprite)) as Sprite;
        // 결과아이템 ID
        slotResultnum = resultItemID;
        // 재료아이템 이미지
        mat_icon.Add(Resources.Load(filename1, typeof(Sprite)) as Sprite);
        mat_icon.Add(Resources.Load(filename2, typeof(Sprite)) as Sprite);
        mat_icon.Add(Resources.Load(filename3, typeof(Sprite)) as Sprite);
    }

    //public Recipe(int id1, int count1, int id2, int count2, int id3, int count3, int money)
    //{
    //    for (int i = 0; i < 3; i++)
    //    {
    //        itemID[i] = new int();
    //        itemCount[i] = new int();
    //    }

    //    // icon = Resources.Load(filename, typeof(Sprite)) as Sprite;
    //    itemID[0] = id1;
    //    itemID[1] = id2;
    //    itemID[2] = id3;
    //    itemCount[0] = count1;
    //    itemCount[1] = count2;
    //    itemCount[2] = count3;
    //    this.money = money;
    //}

    public Recipe Init()
    {
        //Recipe obj = new Recipe(itemID[0], itemCount[0], itemID[1], itemCount[1], itemID[2], itemCount[2], money);
        Recipe obj = new Recipe();
        obj.itemID = itemID;
        obj.itemCount = itemCount;
        obj.money = money;
        obj.icon = icon;
        obj.mat_icon = mat_icon;
        obj.slotResultnum = slotResultnum;
        return obj;
    }

    public void Reset()
    {
        icon = null;
        money = 0;
        itemID = new int[3];
        itemCount = new int[3];
        mat_icon = null;
        slotResultnum = 0;
        for (int i = 0; i < 3; i++)
        {
            itemID[i] = new int();
            itemID[i] = 0;
            itemCount[i] = new int();
            itemCount[i] = 0;
        }
    }
}
