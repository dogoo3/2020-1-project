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

    public Laser laser;
    public Bullet energyBall;
    public EnergyBall magicBall;
    public CircleFloor circle;
    public Fire_Ball fireBall;
    public GameObject switch_bosskey;

    public int boss_poolingCount;
    public int magician_poolingCount;

    private void Awake()
    {
        instance = this;

        for (int i = 0; i < boss_poolingCount; i++)
        {
            Bullet t_object = Instantiate(energyBall, Vector2.zero, Quaternion.identity);
            t_object.transform.parent = gameObject.transform;
            queue_energyball.Enqueue(t_object);
            t_object.gameObject.SetActive(false);

            if (i > magician_poolingCount)
                continue;
            EnergyBall e_object = Instantiate(magicBall, Vector2.zero, Quaternion.identity);
            t_object.transform.parent = gameObject.transform;
            queue_magicBall.Enqueue(e_object);
            t_object.gameObject.SetActive(false);
        }

        Laser temp = Instantiate(laser, Vector2.zero, Quaternion.identity);
        temp.transform.parent = gameObject.transform;
        queue_laser.Enqueue(temp);
        temp.gameObject.SetActive(false);

        //원형 장판
        CircleFloor floorTemp = Instantiate(circle, Vector2.zero, Quaternion.identity);
        floorTemp.transform.parent = gameObject.transform;
        queue_circleFloor.Enqueue(floorTemp);
        floorTemp.gameObject.SetActive(false);

        //불구슬
        Fire_Ball fireBallTemp = Instantiate(fireBall, Vector2.zero, Quaternion.identity);
        fireBallTemp.transform.parent = gameObject.transform;
        queue_fireBall.Enqueue(fireBallTemp);
        fireBallTemp.gameObject.SetActive(false);

        for(int i=0;i<GameManager.instance.playerInfo.Count+1;i++)
        {
            GameObject _switch = Instantiate(switch_bosskey, Vector2.zero, Quaternion.identity);
            _switch.transform.parent = gameObject.transform;
            queue_switch.Enqueue(_switch);
            _switch.gameObject.SetActive(false);
        }
    }

    public void InsertQueue(Bullet _object, Queue<Bullet> _queue) // Second Paramerer is put object Queue(poolingmanager queue)
    {
        _queue.Enqueue(_object);
        _object.gameObject.SetActive(false);
    }
    public Bullet GetQueue(Queue<Bullet> _queue)
    {
        if (queue_energyball.Count != 0)
        {
            Bullet t_object = _queue.Dequeue();
            t_object.gameObject.SetActive(true);
            return t_object;
        }
        return null;
    }

    public void InsertQueue(Laser _object, Queue<Laser> _queue) // Second Paramerer is put object Queue(poolingmanager queue)
    {
        _queue.Enqueue(_object);
        _object.gameObject.SetActive(false);
    }
    public Laser GetQueue(Queue<Laser> _queue)
    {
        if (queue_laser.Count != 0)
        {
            Laser t_object = _queue.Dequeue();
            return t_object;
        }
        return null;
    }

    public void InsertQueue(EnergyBall _object) // Second Paramerer is put object Queue(poolingmanager queue)
    {
        queue_magicBall.Enqueue(_object);
        _object.gameObject.SetActive(false);
    }
    public void GetQueue(Vector2 _direction, Vector2 _transform,string _name)
    {
        if (queue_magicBall.Count != 0)
        {
            EnergyBall t_object = queue_magicBall.Dequeue();
            t_object.gameObject.SetActive(true);
            t_object.ShootBall(_direction, _transform,_name);
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
            CircleFloor t_object = _queue.Dequeue();
            return t_object;
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
            Fire_Ball t_object = _queue.Dequeue();
            return t_object;
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
        if (queue_fireBall.Count != 0)
        {
            GameObject t_object = _queue.Dequeue();
            t_object.transform.position = _position;
            t_object.SetActive(true);
            return t_object;
        }
        return null;
    }
}
