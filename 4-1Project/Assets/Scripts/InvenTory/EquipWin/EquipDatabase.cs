using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipDatabase : MonoBehaviour
{
    public List<EquipInfo> equipInfoList = new List<EquipInfo>();
    
    // itemID, STR, DEF, AttackSpeed, Cooltime
    private void Start()
    {
        // Material Item
        equipInfoList.Add(new EquipInfo(201, 0, 15,0,0.05f));
        equipInfoList.Add(new EquipInfo(202, 0, 0,0,0.2f));
        equipInfoList.Add(new EquipInfo(203, 35, 0,0.2f));
        equipInfoList.Add(new EquipInfo(204, 0, 30));
        equipInfoList.Add(new EquipInfo(205, 0, 0, 0.25f));
        equipInfoList.Add(new EquipInfo(206, 35, 0, 0.2f));
    }
}
