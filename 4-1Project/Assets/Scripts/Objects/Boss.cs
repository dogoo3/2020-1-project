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

    public int HP, STR, DEF;
    private int _fullHp;

    private BossHPBar _BossHPBar;

    public int patternNum;

    public Phase Data;

    public int _circleBullet;

    private void Awake()
    {
        instance = this;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerPos = FindObjectOfType<Player>().transform;
        _BossHPBar = FindObjectOfType<BossHPBar>();
        _fullHp = HP;

        Data.Init(false);
    }

    public void SetHP(JsonData _data)
    {
        HP = int.Parse(_data["Hp"].ToString());
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

    public void ActiveHPBar()
    {
        if(!_BossHPBar.gameObject.activeSelf)
            _BossHPBar.gameObject.SetActive(true);
    }
}
