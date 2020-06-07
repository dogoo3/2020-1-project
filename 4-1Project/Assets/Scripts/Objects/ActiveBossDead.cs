using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBossDead : MonoBehaviour
{
    [Header("이 보스가 사망하면 활성화시킬 오브젝트")]
    public GameObject[] deadActiveObject;
    
    public void ActiveObject()
    {
        for (int i = 0; i < deadActiveObject.Length; i++)
        {
            deadActiveObject[i].SetActive(true);
        }
    }
}
