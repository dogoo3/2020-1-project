using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Magician : MonoBehaviour
{
    public static Player_Magician instance;

    private Player _mainPlayer;

    private Vector2 _mousePos;
    
    private bool _isHit;

    private void Awake()
    {
        instance = this;

        _mainPlayer = GetComponent<Player>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(!_isHit)
            {
                _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _mousePos -= (Vector2)transform.position;
                _mousePos.Normalize();

                _mainPlayer.Data.ax = _mousePos.x;
                _mainPlayer.Data.ay = _mousePos.y;
                
                ObjectPoolingManager.instance.GetQueue(_mousePos, transform.position, gameObject.name);
                _mainPlayer.AttackPlayer();
            }
        }
    }

    public void ActiveAttack(bool _isActive)
    {
        _isHit = _isActive;
    }
}
