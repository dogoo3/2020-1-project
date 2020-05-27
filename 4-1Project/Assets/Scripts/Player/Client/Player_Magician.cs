using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Magician : MonoBehaviour
{
    private Player _mainPlayer;
    private RaycastHit2D _hit2D;
    private Vector2 _mousePos;

    private bool _isHit;

    private float _attacktime;
    private int _layerMask;
    public float attackspeed;

    private bool _isSkill;
    private float _skilltime;
    public float skillcooltime;

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
            if (Time.time - _attacktime > attackspeed)
                _isHit = false;
        }

        if (!_isHit)
        {
            if (Input.GetMouseButton(0))
            {
                _attacktime = Time.time;
                _isHit = true;
                ObjectPoolingManager.instance.GetQueue(_mainPlayer._mousePos, transform.position, gameObject.name);
                _mainPlayer.AttackPlayer(PlayerState.Skill); // 마법사 스킬공격
                _mainPlayer.ChangeAnimationState_Attack();
                _mainPlayer.SendPlayerInfoPacket();
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                _attacktime = Time.time;
                _isHit = true;
                _mainPlayer.AttackPlayer(); // 마법사 기본공격
                _hit2D = Physics2D.Raycast(transform.position, _mainPlayer._mousePos, 2f, _layerMask);
                _mainPlayer.ChangeAnimationState_Attack();

                if (_hit2D.collider != null)
                    _mainPlayer.temp = _hit2D.collider.GetComponent<ItemDropObject>();

                if (_mainPlayer.temp != null) // 채집물에 맞으면
                {
                    _mainPlayer.temp.MinusCount(gameObject.name);
                    if (!_mainPlayer.isGetSwitch) // 스위치를 스폰하지 못했을경우
                        _mainPlayer.SendItemPercentPacket();
                }
            }
        }
        if (_isSkill)
        {
            if (Time.time - _skilltime > skillcooltime)
                _isSkill = false;
        }
        if(!_isSkill)
        {
            if(Input.GetMouseButtonDown(1))
            {
                // 플레이어가 이동 못 하도록 함
                _mainPlayer.ChangePS(PlayerState.Meteor);
                // 3초짜리 메테오 애니메이션
                _isSkill = true;
                // 3초 뒤 메테오 발사
                // 3초 뒤 플레이어 이동 해제
                _mainPlayer.Invoke("Invoke_ChangePSIdle", 3f);
            }
        }
    }
}
