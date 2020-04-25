﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D _rigidbody2D;
    private float _time;
    
    public float lifetime = 2.0f;
    public int power;

    public int STR;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if(Boss.instance != null)
            transform.position = Boss.instance.transform.position;
        _rigidbody2D.velocity = Vector2.zero;
    }

    private void Update()
    {
        _time += Time.deltaTime;
        if (_time > lifetime)
            ObjectPoolingManager.instance.InsertQueue(this,ObjectPoolingManager.instance.queue_energyball);
    }

    private void OnDisable()
    {
        _time = 0;
    }

    public void InduceBullet(Vector2 _dir)
    {
        _rigidbody2D.AddForce(_dir * power);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();
            if(player != null)
            {
                player.Attacked(STR);
                ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_energyball);
                return;
            }
            Player_Server player_Server = collision.GetComponent<Player_Server>();
            if(player_Server != null)
            {
                ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_energyball);
                return;
            }
        }
    }
}