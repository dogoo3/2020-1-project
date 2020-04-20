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

    private float _time;
    public float loadtime;
    public int patternNum;

    public Phase Data;

    private void Awake()
    {
        instance = this;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerPos = FindObjectOfType<Player>().transform;
        _fullHp = 1 / _hp;

        Data.Init(false);
    }

    private void Update()
    {
        _time += 0.032f;
        if (_time > loadtime)
        {
            switch (patternNum)
            {
                case 3: // 유도탄환
                case 4: // 랜덤레이저
                    SendPhaseData();
                    break;
                default:
                    break;
            }
            _time = 0;
        }
    }

    public void SetHP(JsonData _data)
    {
        _hp = int.Parse(_data["Hp"].ToString());
        patternNum = int.Parse(_data["Phase"].ToString());
        Debug.Log(patternNum);
    }

    private void SendPhaseData()
    {
        Data.bx = transform.position.x;
        Data.by = transform.position.y;

        Data.px = _playerPos.transform.position.x;
        Data.py = _playerPos.transform.position.y;

        JsonData SendData = JsonMapper.ToJson(Data);
        ServerClient.instance.Send(SendData.ToString());
    }
}
