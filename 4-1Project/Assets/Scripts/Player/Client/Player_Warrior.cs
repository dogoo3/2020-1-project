﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Warrior : MonoBehaviour
{
    private RaycastHit2D _hit2D;
    private Player _mainPlayer;

    private bool _isHit;
    private float _time;
    public float attackspeed;

    private int _layerMask;

    private void Awake()
    {
        _layerMask = 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("RoomCollider");
        _layerMask = ~_layerMask;
        _mainPlayer = GetComponent<Player>();
    }

    private void Start()
    {
        CharacterInfoWindow.instance.UpdateASPD(attackspeed);
    }

    private void Update()
    {
        if (_isHit)
        {
            _time += Time.deltaTime;
            if (_time > attackspeed)
            {
                _time = 0;
                _isHit = false;
            }
        }

        if (!_isHit)
        {
            if (Input.GetMouseButtonDown(0)) 
            {
                _mainPlayer.AttackPlayer();
                _isHit = true;
                _hit2D = Physics2D.Raycast(transform.position, _mainPlayer._mousePos, 2f, _layerMask);
                _mainPlayer.ChangeAnimationState_Attack();
                _mainPlayer.SendPlayerInfoPacket();
                if (_hit2D.collider != null)
                {
                    ItemDropObject temp;

                    if (_hit2D.collider.name == "Boss") // 보스에 맞으면
                    {
                        _mainPlayer.SendDamageInfo(Boss.instance.DEF);
                        Boss.instance.ActiveHPBar();
                    }

                    if (_hit2D.collider.gameObject.tag == "FireBall")
                        Boss.instance._fireBall.HitFireBall(_hit2D.collider.gameObject.name);
                    
                    temp = _hit2D.collider.GetComponent<ItemDropObject>();

                    if (temp != null) // 채집물에 맞으면
                    {
                        temp.MinusCount();
                    }
                }
            }
        }
    }
}
