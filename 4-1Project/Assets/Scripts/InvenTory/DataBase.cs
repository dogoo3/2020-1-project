using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    public List<Item> itemList = new List<Item>();

    // ItemID , ItemName, ItemDes, ItemSpriteFilename
    private void Start()
    {
        // Material Item
        itemList.Add(new Item(1, "50원", "50원입니다.", "50WON"));
        itemList.Add(new Item(2, "100원", "100원입니다.", "100WON"));
        itemList.Add(new Item(3, "500원", "500원입니다.", "500WON"));
        itemList.Add(new Item(4, "골드바", "매우 비싼 골드바", "goldbar"));

        // Potion Item
        itemList.Add(new Item(101, "빨간 포션", "HP가 회복되게 할 겁니다", "RedPotion"));
        itemList.Add(new Item(102, "파워 엘릭서", "짱짱하게 회복합니다", "PowerElixir"));

        // Equipment Item
        itemList.Add(new Item(201, "법전", "법전으로 사람을 때려도 무죄입니다", "LawBook"));
        itemList.Add(new Item(202, "샷건", "흔하게 볼 수 있는 총입니다", "ShotGun"));
    }
}
