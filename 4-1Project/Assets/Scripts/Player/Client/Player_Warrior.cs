using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player_Warrior : MonoBehaviour
{
    private Player _mainPlayer;
    private SkillCooltimeController _cooltimecontroller;

    private RaycastHit2D _hit2D;

    private bool _isHit, _isSkill;
    private int _layerMask;
    private float _attackcooltime, _skillcooltime;

    public GameObject invincibleWall; // 무적 방어벽

    public float attackspeed, skillcooltime;

    private void Awake()
    {
        _layerMask = 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("RoomCollider");
        _layerMask = ~_layerMask;
        _mainPlayer = GetComponent<Player>();
        _cooltimecontroller = GameObject.Find("SkillIcon").transform.Find("CooltimePanel").GetComponent<SkillCooltimeController>();
    }

    private void Start()
    {
        CharacterInfoWindow.instance.UpdateASPD(attackspeed);
        _cooltimecontroller.SetCooltime(skillcooltime);
    }

    private void Update()
    {
        if (_mainPlayer.playerState == PlayerState.Restriction)
            return;

        #region Attack
        if (_isHit) // 공격쿨타임 중
        {
            if (Time.time - _attackcooltime > attackspeed)
                _isHit = false;
        }

        if (!_isHit) // 공격쿨타임 종료
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_mainPlayer.CheckClickInv())
                    return;

                _mainPlayer.AttackPlayer();
                _isHit = true;
                _hit2D = Physics2D.Raycast(transform.position, _mainPlayer._mousePos, 2f, _layerMask);
                _mainPlayer.ChangeAnimationState_Attack();
                _attackcooltime = Time.time;

                SoundManager.instance.PlaySFX("1PC_Swing_9");

                if (_hit2D.collider != null)
                {
                    if (_hit2D.collider.tag == "Boss") // 보스에 맞으면
                    {
                        _mainPlayer.SendDamageInfo(Boss.instance.DEF);
                        Boss.instance.ActiveHPBar();
                        SoundManager.instance.PlaySFX("1PC_Enemy_Hit2");
                    }

                    if (_hit2D.collider.gameObject.tag == "FireBall") // 보스가 소환한 불구슬에 맞으면
                    {
                        Boss.instance._fireBall.HitFireBall(_hit2D.collider.gameObject.name);
                        SoundManager.instance.PlaySFX("1PC_Enemy_Hit2");
                    }

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
        #endregion
        #region Skill
        if(_isSkill) // 스킬 쿨타임 중
        {
            if (Time.time - _skillcooltime > skillcooltime)
            {
                _isSkill = false;
                _cooltimecontroller.EndCooltime();
            }
            _cooltimecontroller.ShowCooltime(Time.time - _skillcooltime);
        }
        if(!_isSkill) // 스킬 쿨타임 종료(스킬발동)
        {
            if(Input.GetMouseButtonDown(1))
            {
                if (_mainPlayer.CheckClickInv())
                    return;

                _skillcooltime = Time.time; // 스킬발동시간 기록
                _mainPlayer.DEF *= 2; // 방어력 X2
                CharacterInfoWindow.instance.UpdateDEF(_mainPlayer.DEF);
                _mainPlayer.AttackPlayer(PlayerState.Invincible);
                invincibleWall.SetActive(true);
                // 무적 이펙트 발동
                _isSkill = true;
                // 사운드 실행
                SoundManager.instance.PlaySFX("1PC_Skill_3");
                // 스킬발동 후 해제
                _mainPlayer.Invoke("Invoke_ChangePSIdle", 1f); // 무적 상태 해제
                Invoke("Invoke_OffEffect", 1f);
                _mainPlayer.Invoke("Invoke_DivideDEF", 15f); // 방어력 원상복구
            }
        }
        #endregion
    }

    #region Invoke
    private void Invoke_OffEffect()
    { 
        invincibleWall.SetActive(false);
        // 이펙트 해제
    }
    #endregion
}
