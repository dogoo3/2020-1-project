using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipDatabase : MonoBehaviour
{
    public List<EquipInfo> equipInfoList = new List<EquipInfo>();
    
    // itemID, STR, DEF
    private void Start()
    {
        // Material Item
        equipInfoList.Add(new EquipInfo(201, 0, 23));
        equipInfoList.Add(new EquipInfo(202, 150, 0));
        equipInfoList.Add(new EquipInfo(203, 0, 0));
    }
}
