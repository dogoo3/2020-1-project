using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class FireBallData : MonoBehaviour
{
    FireBallState _data_damageFireBall;

    public int HitCount = 3;
    public int MaxCount = 3;

    bool _serverDamageSetOn;
    private void Awake()
    {
        MaxCount = HitCount;
    }

    private void OnDisable()
    {
        HitCount = MaxCount;
    }

    public void ResetCount()
    {
        HitCount = MaxCount;
        this.gameObject.SetActive(true);
    }
  
    public void SetCount()
    {
        if(HitCount ==0)
        {
            _data_damageFireBall.Init(this.gameObject.name, true);
            JsonData SendData = JsonMapper.ToJson(_data_damageFireBall);
            ServerClient.instance.Send(SendData.ToString());
            this.gameObject.SetActive(false);          
        }
        else
        { 
            HitCount -= 1;
            _data_damageFireBall.Init(this.gameObject.name,false);
            JsonData SendData = JsonMapper.ToJson(_data_damageFireBall);
            ServerClient.instance.Send(SendData.ToString());
        }
    }

    private void Update()
    {
        if (HitCount == 0)
        {
            this.gameObject.SetActive(false);
        }

    }

    public void ServerSetCount()
    {
        HitCount -= 1;
    }
}
