using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Magician : MonoBehaviour
{
    public static Player_Magician instance;

    private Player _mainPlayer;
    private RaycastHit2D _hit2D;
    private Vector2 _mousePos;
    private ItemDropObject temp;

    private bool _isHit;

    private void Awake()
    {
        instance = this;

        _mainPlayer = GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!_isHit)
            {
                //_mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //_mousePos -= (Vector2)transform.position;
                //_mousePos.Normalize();

                ObjectPoolingManager.instance.GetQueue(_mainPlayer._mousePos, transform.position, gameObject.name);
                _mainPlayer.AttackPlayer(PlayerState.Skill); // 마법사 스킬공격
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!_isHit)
            {
                _mainPlayer.AttackPlayer(); // 마법사 기본공격
                _hit2D = Physics2D.Raycast(transform.position, _mainPlayer._mousePos, 2f);

                if(_hit2D.collider != null)
                    temp = _hit2D.collider.GetComponent<ItemDropObject>();

                if (temp != null) // 채집물에 맞으면
                    temp.MinusCount();
            }
        }
    }

    public void ActiveAttack(bool _isActive)
    {
        _isHit = _isActive;
    }
}
