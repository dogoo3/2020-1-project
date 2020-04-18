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

    private void Start()
    {
        
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _mainPlayer.AttackPlayer();
            _hit2D = Physics2D.Raycast(transform.position,_mainPlayer._mousePos,2f);

            if (_hit2D.collider != null)
            {
                if (_hit2D.collider.name == "Boss")
                {
                    _mainPlayer.SendDamageInfo();
                }
            }
        }
    }
}
