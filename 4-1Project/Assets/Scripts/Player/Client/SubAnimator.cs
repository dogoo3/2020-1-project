using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubAnimator : MonoBehaviour
{
    /* Private */

    // Unity Components
    Animator _animator;
    SpriteRenderer[] _characterSprite;

    // Scripts
    public Player_Warrior player_Warrior;
    public Player_Magician player_Magician;
    // Unity Keywords

    // Variables
    private bool _walk;
    private bool _attack;
    /* Public */

    // Unity Components

    // Scripts

    // Unity Keywords

    // Variables
    public bool active;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _characterSprite = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        IsActive(active);
    }

    public void Move(bool _state)
    {
        _animator.SetBool("Walk", _state);
    }

    public void IsActive(bool _state)
    {
        if (_characterSprite[0].gameObject.activeSelf == _state)
            return;
        for (int i = 0; i < _characterSprite.Length; i++)
            _characterSprite[i].gameObject.SetActive(_state);
    }

    public void Attack()
    {
        _animator.SetTrigger("Attack");
    }

    public void Attacked(bool _isAttacked)
    {
        _animator.SetBool("Attacked", _isAttacked);
        DisableAttack(); // 애니메이션 공격 bool변수가 true에서 멈추는 것을 방지하기 위해서 강제로 false로 만들어버림.
    }

    public void EnableAttack() // 애니메이션 이벤트 함수
    {
        if (player_Warrior == null && player_Magician == null)
            return;

        if (player_Warrior != null)
            player_Warrior.ActiveAttack(true);
        else
            player_Magician.ActiveAttack(true);
    }

    public void DisableAttack() // 애니메이션 이벤트 함수
    {
        if (player_Warrior == null && player_Magician == null)
            return;

        if (player_Warrior != null)
            player_Warrior.ActiveAttack(false);
        else
            player_Magician.ActiveAttack(false);
    }


}
