using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class Boss : MonoBehaviour
{
    public static Boss instance;
    Rigidbody2D _rigidbody2D;

    private float _Nowpercent;

    private int _hp = 100;
    private float _fullHp;

    private int _patternNum;

    public Phase Data;

    private void Awake()
    {
        instance = this;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _fullHp = 1 / _hp;
    }

    private void Update()
    {
        switch(_patternNum)
        {
        }
    }

    public void SetHP(JsonData _data)
    {
        _hp = int.Parse(_data["Hp"].ToString());
        _patternNum = int.Parse(_data["Phase"].ToString());
        Debug.Log(_patternNum);
    }
}
