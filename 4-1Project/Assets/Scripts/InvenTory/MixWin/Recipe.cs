using UnityEngine;
using System.Collections;

[System.Serializable]
public class Recipe
{
    public Sprite icon;
    public Sprite[] mat_icons = new Sprite[3];
    public int[] itemID = new int[3];
    public int[] itemCount = new int[3];
    public int money;
    public int slotResultnum;

    public Recipe()
    {
        icon = null;
        for(int i=0;i<3;i++)
        {
            itemID[i] = new int();
            itemCount[i] = new int();
            mat_icons[i] = null;
        }
        money = 0;
    }

    public Recipe(int id1, int count1, int id2, int count2, int id3, int count3, int money)
    {
        for (int i = 0; i < 3; i++)
        {
            itemID[i] = new int();
            itemCount[i] = new int();
            mat_icons[i] = null;
        }

        // icon = Resources.Load(filename, typeof(Sprite)) as Sprite;
        itemID[0] = id1;
        itemID[1] = id2;
        itemID[2] = id3;
        itemCount[0] = count1;
        itemCount[1] = count2;
        itemCount[2] = count3;
        this.money = money;
    }

    public Recipe Init()
    {
        Recipe obj = new Recipe(itemID[0], itemCount[0], itemID[1], itemCount[1], itemID[2], itemCount[2], money);
        obj.itemID = itemID;
        obj.itemCount = itemCount;
        obj.money = money;
        obj.icon = icon;
        obj.mat_icons = mat_icons;
        obj.slotResultnum = slotResultnum;
        return obj;
    }

    public void Reset()
    {
        icon = null;
        money = 0;
        itemID = new int[3];
        itemCount = new int[3];
        mat_icons = new Sprite[3];
        for (int i = 0; i < 3; i++)
        {
            itemID[i] = new int();
            itemID[i] = 0;
            itemCount[i] = new int();
            itemCount[i] = 0;
            mat_icons[i] = null;
        }
    }
}
