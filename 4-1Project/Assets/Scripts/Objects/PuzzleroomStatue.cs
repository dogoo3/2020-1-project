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
            Chatting.instance.PutSystemMessage("조각상 : 어둠의 외진 곳으로 가보거라.");
    }

    public void DestroyStatue()
    {
        SoundManager.instance.PlaySFX("Statue_Broken_9");
        _animator.enabled = true;
    }
}
