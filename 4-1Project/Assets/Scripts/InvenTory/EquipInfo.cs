using UnityEngine;
using System.Collections;

[System.Serializable]
public class EquipInfo
{
    public int itemID; // 아이템 번호
    public int STR; // 아이템 공격력
    public int DEF; // 아이템 방어력

    public EquipInfo()
    {
        itemID = 0;
        STR = 0;
        DEF = 0;
    }

    public EquipInfo(int _itemID, int _STR, int _DEF) // Item 클래스에 대한 생성자
    {
        itemID = _itemID;
        STR = _STR;
        DEF = _DEF;
    }

    public EquipInfo Init() // 깊은 복사
    {
        EquipInfo obj = new EquipInfo(itemID, STR, DEF);
        obj.itemID = itemID;
        obj.STR = STR;
        obj.DEF = DEF;
        return obj;
    }
}
