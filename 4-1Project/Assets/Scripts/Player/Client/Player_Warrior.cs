using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Warrior : MonoBehaviour
{
    private RaycastHit2D _hit2D;
    private Player _mainPlayer;

    private bool _isHit;

    private void Awake()
    {
        _mainPlayer = GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!_isHit)
            {
                _mainPlayer.AttackPlayer();

                _hit2D = Physics2D.Raycast(transform.position, _mainPlayer._mousePos, 2f);

                if (_hit2D.collider != null)
                {
                    ItemDropObject temp;

                    if (_hit2D.collider.name == "Boss") // 보스에 맞으면
                    {
                        _mainPlayer.SendDamageInfo(Boss.instance.DEF);
                        Boss.instance.ActiveHPBar();
                    }

                    temp = _hit2D.collider.GetComponent<ItemDropObject>();

                    if (temp != null) // 채집물에 맞으면
                    {
                        temp.MinusCount();
                    }
                }
            }
        }
    }

    public void ActiveAttack(bool _isActive)
    {
        _isHit = _isActive;
    }
}
