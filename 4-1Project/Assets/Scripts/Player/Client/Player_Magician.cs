using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player_Magician : MonoBehaviour
{
    private Player _mainPlayer;

    private Vector2 _mousePos;
    private RaycastHit2D _hit2D;

    private bool _isHit, _isSkill;
    private int _layerMask;
    private float _attackcooltime, _skillcooltime;

    private void Awake()
    {
        _layerMask = 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("RoomCollider");
        _layerMask = ~_layerMask;

        _mainPlayer = GetComponent<Player>();
    }

    private void Start()
    {
        CharacterInfoWindow.instance.UpdateASPD(_mainPlayer.attackcooltime);
        _mainPlayer.Data.attackspeed = 1.2f;
    }

    private void Update()
    {
        if (_mainPlayer.playerState == PlayerState.Restriction) // 속박상태이면 어떤 행동도 할 수 없음.
            return;

        if (_isHit)
        {
            if (Time.time - _attackcooltime > _mainPlayer.attackcooltime)
                _isHit = false;
        }

        if (!_isHit)
        {
            if (Time.time - _skillcooltime > 3.0f) // 메테오를 시전 중일 때는 기본공격 및 스킬공격 불가능
            {
                if (Input.GetMouseButton(0))
                {
                    if (_mainPlayer.CheckClickInv())
                        return;

                    _attackcooltime = Time.time;
                    _isHit = true;
                    ObjectPoolingManager.instance.GetQueue(_mainPlayer._mousePos, transform.position, gameObject.name);
                    _mainPlayer.AttackPlayer(PlayerState.Skill); // 마법사 스킬공격
                    SoundManager.instance.PlaySFX("2PC_Skill_16");
                    _mainPlayer.ChangeAnimationState_Attack();
                    _mainPlayer.SendPlayerInfoPacket();
                }
                if (Input.GetKeyDown(KeyCode.F))
                {
                    _attackcooltime = Time.time;
                    _isHit = true;
                    _mainPlayer.AttackPlayer(); // 마법사 기본공격
                    _hit2D = Physics2D.Raycast(transform.position, _mainPlayer._mousePos, 2f, _layerMask);
                    _mainPlayer.ChangeAnimationState_Attack();
                    SoundManager.instance.PlaySFX("1PC_Swing_9");

                    if (_hit2D.collider != null)
                    {
                        _mainPlayer.temp = _hit2D.collider.GetComponent<ItemDropObject>();

                        if (_mainPlayer.temp != null) // 채집물에 맞으면
                        {
                            _mainPlayer.temp.MinusCount(gameObject.name);
                            if (!_mainPlayer.isGetSwitch && _mainPlayer.temp.CheckCount()) // 스위치를 스폰하지 못했을경우, 아이템 카운트가 0인 경우
                                _mainPlayer.SendItemPercentPacket();
                        }
                    }
                    
                }
            }
        }
        if (_isSkill)
        {
            if (Time.time - _skillcooltime > _mainPlayer.skillcooltime)
            {
                _isSkill = false;
                _mainPlayer._cooltimecontroller.EndCooltime();
            }
            _mainPlayer._cooltimecontroller.ShowCooltime(Time.time - _skillcooltime);
        }
        if (!_isSkill)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (_mainPlayer.CheckClickInv())
                    return;

                _mainPlayer.ChangeAnimationState_Meteor(); // 3초짜리 메테오 애니메이션
                _mainPlayer.AttackPlayer(PlayerState.Meteor);
                _skillcooltime = Time.time;
                _isSkill = true;
                // 플레이어가 이동 못 하도록 함
                Invoke("ShootMeteor", 3.0f);  // 3초 뒤 메테오 발사
                _mainPlayer.Invoke("Invoke_ChangePSIdle", 3.0f); // 3초 뒤 플레이어 이동 해제
            }
        }
    }
    #region Invoke
    private void ShootMeteor()
    {
        ObjectPoolingManager.instance.GetQueue_meteor(_mainPlayer._mousePos, transform.position, gameObject.name);
        SoundManager.instance.PlaySFX("2PC_Skill_22");
    }
    #endregion
}
