using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleroomStatue : MonoBehaviour
{
    Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == GameManager.instance.PlayerName)
            Chatting.instance.PutSystemMessage("조각상 : 하나의 충돌은 셋의 행성을 파괴하고 하나의 헤븐헤르는 두개의 행성의 마지막에서 시작되었다.\n- 헤븐헤르의 시작 -");
    }

    public void DestroyStatue()
    {
        SoundManager.instance.PlaySFX("Statue_Broken_9");
        _animator.enabled = true;
    }
}
