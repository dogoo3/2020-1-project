﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager instance;

    public Queue<Laser> queue_laser = new Queue<Laser>();
    public Queue<Bullet> queue_energyball = new Queue<Bullet>();
    public Queue<EnergyBall> queue_magicBall = new Queue<EnergyBall>();
    public Queue<CircleFloor> queue_circleFloor = new Queue<CircleFloor>();
    public Queue<Fire_Ball> queue_fireBall = new Queue<Fire_Ball>();
    public Queue<GameObject> queue_switch = new Queue<GameObject>();
    public Queue<Restriction> queue_restriction = new Queue<Restriction>();
    public Queue<Meteor> queue_meteor = new Queue<Meteor>();
    public Queue<InduceMissile> queue_inducemissile = new Queue<InduceMissile>();

    [Header("생성할 오브젝트 배치")]
    public Laser laser;
    public Bullet energyBall;
    public EnergyBall magicBall;
    public CircleFloor circle;
    public Fire_Ball fireBall;
    public GameObject switch_bosskey;
    public Restriction restriction;
    public Meteor meteor;
    public InduceMissile inducemissile;

    public int boss_poolingCount;
    public int magician_poolingCount;

    //풀링한 오브젝트를 임시로 저장하는 private 변수
    Bullet t_object;
    EnergyBall e_object;
    Laser lasertemp;
    CircleFloor floorTemp;
    Fire_Ball fireBallTemp;
    GameObject _switch;
    Restriction _restrict;
    Meteor _meteor;
    InduceMissile _induceMissile;
    
    private void Awake()
    {
        instance = this;

        for (int i = 0; i < boss_poolingCount; i++)
        {
            t_object = Instantiate(energyBall, Vector2.zero, Quaternion.identity);
            t_object.transform.parent = gameObject.transform;
            t_object.name = "Bullet_" + i.ToString();
            queue_energyball.Enqueue(t_object);
            t_object.gameObject.SetActive(false);

            if (i > magician_poolingCount)
                continue;
            e_object = Instantiate(magicBall, Vector2.zero, Quaternion.identity);
            e_object.transform.parent = gameObject.transform;
            e_object.name = "energyBall_" + i.ToString();
            queue_magicBall.Enqueue(e_object);
            e_object.gameObject.SetActive(false);
        }

        // 레이저
        lasertemp = Instantiate(laser, Vector2.zero, Quaternion.identity);
        lasertemp.transform.parent = gameObject.transform;
        lasertemp.name = "lasertemp1";
        queue_laser.Enqueue(lasertemp);
        lasertemp.gameObject.SetActive(false);

        //원형 장판
        floorTemp = Instantiate(circle, Vector2.zero, Quaternion.identity);
        floorTemp.transform.parent = gameObject.transform;
        floorTemp.name = "floorTemp1";
        queue_circleFloor.Enqueue(floorTemp);
        floorTemp.gameObject.SetActive(false);

        //불구슬
        fireBallTemp = Instantiate(fireBall, Vector2.zero, Quaternion.identity);
        fireBallTemp.transform.parent = gameObject.transform;
        fireBallTemp.name = "fireBallTemp1";
        queue_fireBall.Enqueue(fireBallTemp);
        fireBallTemp.gameObject.SetActive(false);

        // 스위치 및 속박
        for (int i = 0; i < GameManager.instance.playerInfo.Count + 1; i++)
        {
            _switch = Instantiate(switch_bosskey, Vector2.zero, Quaternion.identity);
            _switch.transform.parent = gameObject.transform;
            _switch.name = "switch_" + i.ToString();
            queue_switch.Enqueue(_switch);
            _switch.gameObject.SetActive(false);

            _restrict = Instantiate(restriction, Vector2.zero, Quaternion.identity);
            _restrict.transform.parent = gameObject.transform;
            _restrict.name = "restrict_" + i.ToString();
            queue_restriction.Enqueue(_restrict);
            _restrict.gameObject.SetActive(false);

            _meteor = Instantiate(meteor, Vector2.zero, Quaternion.identity);
            _meteor.transform.parent = gameObject.transform;
            _meteor.name = "meteor_" + i.ToString();
            queue_meteor.Enqueue(_meteor);
            _meteor.gameObject.SetActive(false);

            _induceMissile = Instantiate(inducemissile, Vector2.zero, Quaternion.identity);
            _induceMissile.name = "Missile_" + i.ToString();
            _induceMissile.transform.parent = gameObject.transform;
            // induceMissile 스크립트 자체가 한번 실행되면서 넣기 때문에 또 넣으면 안 됨.
            _induceMissile.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        for (int i = 0; i < GameManager.instance.playerInfo.Count + 1; i++)
        {
            _induceMissile = queue_inducemissile.Dequeue();
            Debug.Log(_induceMissile.name);
            if (i == 0) // 나에게 오는 유도미사일
            {
                Debug.Log("미사일 객체 클라이언트");
                Debug.Log(GameManager.instance._player.transform);
                _induceMissile.SetInduceObject(GameManager.instance._player.transform);
            }
            else // 서버 플레이어에게 가는 유도미사일
            {
                Debug.Log("미사일 객체 서버");
               _induceMissile.SetInduceObject(OtherPlayerManager.instance.PlayerList[OtherPlayerManager.instance.s_playerlist[i - 1]].transform);
            }
            queue_inducemissile.Enqueue(_induceMissile);
        }
    }

    // 보스 총알 풀링 
    public void InsertQueue(Bullet _object, Queue<Bullet> _queue) // Second Paramerer is put object Queue(poolingmanager queue)
    {
        _queue.Enqueue(_object);
        _object.gameObject.SetActive(false);
    }
    public Bullet GetQueue(Queue<Bullet> _queue)
    {
        if (queue_energyball.Count != 0)
        {
            t_object = _queue.Dequeue();
            t_object.gameObject.SetActive(true);
            return t_object;
        }
        return null;
    }

    // 레이저 풀링
    public void InsertQueue(Laser _object, Queue<Laser> _queue) // Second Paramerer is put object Queue(poolingmanager queue)
    {
        _queue.Enqueue(_object);
        _object.gameObject.SetActive(false);
    }
    public Laser GetQueue(Queue<Laser> _queue)
    {
        if (queue_laser.Count != 0)
        {
            lasertemp = _queue.Dequeue();
            return lasertemp;
        }
        return null;
    }

    // 에너지볼 풀링(법사 공격스킬)
    public void InsertQueue(EnergyBall _object) // Second Paramerer is put object Queue(poolingmanager queue)
    {
        queue_magicBall.Enqueue(_object);
        _object.gameObject.SetActive(false);
    }
    public void GetQueue(Vector2 _direction, Vector2 _transform,string _name)
    {
        if (queue_magicBall.Count != 0)
        {
            e_object = queue_magicBall.Dequeue();
            e_object.gameObject.SetActive(true);
            e_object.ShootBall(_direction, _transform,_name);
        }
    }
   
    //원형 장판 풀링
    public void InsertQueue(CircleFloor _object, Queue<CircleFloor> _queue) // Second Paramerer is put object Queue(poolingmanager queue)
    {
        _queue.Enqueue(_object);
        _object.gameObject.SetActive(false);
    }
    public CircleFloor GetQueue(Queue<CircleFloor> _queue)
    {
        if (queue_circleFloor.Count != 0)
        {
            floorTemp = _queue.Dequeue();
            return floorTemp;
        }
        return null;
    }

    //불구슬 풀링
    public void InsertQueue(Fire_Ball _object, Queue<Fire_Ball> _queue) // Second Paramerer is put object Queue(poolingmanager queue)
    {
        _queue.Enqueue(_object);
        _object.gameObject.SetActive(false);
    }
    public Fire_Ball GetQueue(Queue<Fire_Ball> _queue)
    {
        if (queue_fireBall.Count != 0)
        {
            fireBallTemp = _queue.Dequeue();
            return fireBallTemp;
        }
        return null;
    }

    // 스위치 풀링
    public void InsertQueue(GameObject _object, Queue<GameObject> _queue) // Second Paramerer is put object Queue(poolingmanager queue)
    {
        _queue.Enqueue(_object);
        _object.gameObject.SetActive(false);
    }
    public GameObject GetQueue(Queue<GameObject> _queue, Vector2 _position)
    {
        if (queue_switch.Count != 0)
        {
            _switch = _queue.Dequeue();
            _switch.transform.position = _position + new Vector2(0, 2);
            _switch.SetActive(true);
            return _switch;
        }
        return null;
    }

    // 속박 풀링
    public void InsertQueue(Restriction _object, Queue<Restriction> _queue) // Second Paramerer is put object Queue(poolingmanager queue)
    {
        _queue.Enqueue(_object);
        _object.gameObject.SetActive(false);
    }
    public Restriction GetQueue(Queue<Restriction> _queue)
    {
        if(queue_restriction.Count != 0)
        {
            _restrict = _queue.Dequeue();
            return _restrict;
        }
        return null;
    }

    // 메테오 풀링
    public void InsertQueue(Meteor _object) // Second Paramerer is put object Queue(poolingmanager queue)
    {
        queue_meteor.Enqueue(_object);
        _object.gameObject.SetActive(false);
    }
    public void GetQueue_meteor(Vector2 _direction, Vector2 _transform, string _name)
    {
        if (queue_meteor.Count != 0)
        {
            _meteor = queue_meteor.Dequeue();
            _meteor.gameObject.SetActive(true);
            _meteor.ShootBall(_direction, _transform, _name);
        }
    }

    // 유도미사일 풀링
    public void InsertQueue(InduceMissile _object) // Second Paramerer is put object Queue(poolingmanager queue)
    {
        queue_inducemissile.Enqueue(_object);
        _object.gameObject.SetActive(false);
    }
    public InduceMissile GetQueue(Queue<InduceMissile> _queue)
    {
        if (queue_inducemissile.Count != 0)
        {
            _induceMissile = _queue.Dequeue();
            _induceMissile.gameObject.SetActive(true);
            return _induceMissile;
        }
        return null;
    }
}
