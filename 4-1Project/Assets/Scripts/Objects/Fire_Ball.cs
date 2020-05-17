using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Ball : MonoBehaviour
{
    public List<FireBallData> _fireBallDatas;
    public float speed = 45.0f;

    int time = 0;

    string Name;
    bool _searchOn;

    public float _lifeTime = 4.5f;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime * speed);

        if(_searchOn)
        {
            _searchOn = false;
            for (int i = 0; i < _fireBallDatas.Count; ++i)
            {
                if (_fireBallDatas[i].gameObject.name == Name)
                {
                    _fireBallDatas[i].ServerSetCount();
                }
            }
        }

        if (Boss.instance.HP <= 0)
            gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        for (int i = 0; i < _fireBallDatas.Count; ++i)
        {
            _fireBallDatas[i].ResetCount();
        }

        if (PatternManager.instance != null)
        {
            transform.position = PatternManager.instance.transform.position;
            //transform.Rotate(0, 0, time * speed);
            PatternManager.instance.DelayPhaseTimeEnd(_lifeTime);
        }       
    }

    private void OnDisable()
    {
        if (ObjectPoolingManager.instance != null)
        {
            ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_fireBall);
            Debug.Log("불구슬 큐에 들어감");
        }
        transform.position = Vector2.zero;

        if (PatternManager.instance != null)
            PatternManager.instance.TimeDelaySendDelayPhaseEnd(0.0f);
    }

    //내 캐릭터 전용
    public void HitFireBall(string _name)
    {
        for (int i = 0; i < _fireBallDatas.Count; ++i)
        {
            if(_fireBallDatas[i].gameObject.name == _name)
            {
                _fireBallDatas[i].SetCount();
            }
        }
    }

    //서버 전용 파이어볼 데미지
    public void ServerHitFireBall(string _name)
    {
        Name = _name;
        _searchOn = true;
    }

    public void CalcTime(int _time)
    {
        time =((DateTime.Now.Hour * 60 * 60 * 60) + (DateTime.Now.Minute * 60 * 60) + (DateTime.Now.Second * 60))
         * 1000 + DateTime.Now.Millisecond;
        time -= _time;
    }

    public void CalcFireBall()
    {
        for (int i = 0; i < _fireBallDatas.Count; ++i)
        {
            if (_fireBallDatas[i].gameObject.activeSelf)
            {
                Boss.instance.SetFullHP();
                break;
            }
        }
        gameObject.SetActive(false);
    }
}
