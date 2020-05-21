using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Magician : MonoBehaviour
{
    private Player _mainPlayer;
    private RaycastHit2D _hit2D;
    private Vector2 _mousePos;
    private ItemDropObject temp;

    private bool _isHit;

    private float _time;
    private int _layerMask;
    public float attackspeed;

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
            if (Input.GetMouseButton(0))
            {
                _isHit = true;
                ObjectPoolingManager.instance.GetQueue(_mainPlayer._mousePos, transform.position, gameObject.name);
                _mainPlayer.AttackPlayer(PlayerState.Skill); // 마법사 스킬공격
                _mainPlayer.ChangeAnimationState_Attack();
                _mainPlayer.SendPlayerInfoPacket();
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                _isHit = true;
                _mainPlayer.AttackPlayer(); // 마법사 기본공격
                _hit2D = Physics2D.Raycast(transform.position, _mainPlayer._mousePos, 2f, _layerMask);
                _mainPlayer.ChangeAnimationState_Attack();

                if (_hit2D.collider != null)
                    temp = _hit2D.collider.GetComponent<ItemDropObject>();

                if (temp != null) // 채집물에 맞으면
                {
                    temp.MinusCount(gameObject.name);
                    if (!_mainPlayer.isGetSwitch) // 스위치를 스폰하지 못했을경우
                    {
                        if (Random.Range(0, 10) >= 5) // 스위치 스폰 X(70%)
                        {
                            temp.ChangeSpawnSwitchState(false);
                            _mainPlayer.Data.switchstate = false;
                        }
                        else // 스위치 스폰 O(50%)
                        {
                            temp.ChangeSpawnSwitchState(true);
                            _mainPlayer.Data.switchstate = true;
                        }
                    }
                }
                _mainPlayer.SendPlayerInfoPacket();
            }
        }
    }
}
