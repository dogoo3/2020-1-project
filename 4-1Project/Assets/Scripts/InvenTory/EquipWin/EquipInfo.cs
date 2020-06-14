using UnityEngine;
using System.Collections;

[System.Serializable]
public class EquipInfo
{
    public int itemID; // 아이템 번호
    public int STR; // 아이템 공격력
    public int DEF; // 아이템 방어력
    public float AttackSpeed; // 공격속도
    public float cooltimePer; // 쿨타임 감소비율

    public EquipInfo()
    {
        itemID = 0;
        STR = 0;
        DEF = 0;
        AttackSpeed = 0;
        cooltimePer = 0;
    }

    public EquipInfo(int _itemID=0, int _STR=0, int _DEF=0, float _AttackSpeed = 0, float _cooltimePer = 0) // Item 클래스에 대한 생성자
    {
        itemID = _itemID;
        STR = _STR;
        DEF = _DEF;
        AttackSpeed = _AttackSpeed;
        cooltimePer = _cooltimePer;
    }

    public EquipInfo Init() // 깊은 복사
    {
        EquipInfo obj = new EquipInfo(itemID, STR, DEF, AttackSpeed, cooltimePer);
        obj.itemID = itemID;
        obj.STR = STR;
        obj.DEF = DEF;
        obj.AttackSpeed = AttackSpeed;
        obj.cooltimePer = cooltimePer;
        return obj;
    }
}
