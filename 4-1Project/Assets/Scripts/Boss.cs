using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class Boss : MonoBehaviour
{
    public static Boss instance;

    Rigidbody2D _rigidbody2D;
    Transform _playerPos;

    private float _Nowpercent;

    private int _hp = 100;
    private float _fullHp;

    public int patternNum;

    public Phase Data;

    public int _circleBullet;

    private void Awake()
    {
        instance = this;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerPos = FindObjectOfType<Player>().transform;
        _fullHp = 1 / _hp;

        Data.Init(false);
    }

    public void SetHP(JsonData _data)
    {
        _hp = int.Parse(_data["Hp"].ToString());
        if (patternNum != int.Parse(_data["Phase"].ToString()))
        {
            patternNum = int.Parse(_data["Phase"].ToString());
            PatternManager.instance._isStart = true;
        }
        else
        {
            patternNum = int.Parse(_data["Phase"].ToString());
        }

        Debug.Log(patternNum);
    }

    public void DelaySendPhaseData(float _delayTime)
    {
        Invoke("SendPhaseData", _delayTime);
    }

    private void SendPhaseData()
    {
        CancelInvoke("SendPhaseData");
        Data.bx = transform.position.x;
        Data.by = transform.position.y;

        Data.px = _playerPos.transform.position.x;
        Data.py = _playerPos.transform.position.y;

        JsonData SendData = JsonMapper.ToJson(Data);
        ServerClient.instance.Send(SendData.ToString());
    }
}
