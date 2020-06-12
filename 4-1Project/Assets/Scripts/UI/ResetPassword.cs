using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPassword : MonoBehaviour
{
    private SpriteRenderer _spriterenderer;
    private bool _onButton;

    public Sprite nononButton;
    public Sprite onButton;

    private void Awake()
    {
        _spriterenderer = GetComponent<SpriteRenderer>();    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_onButton)
            return;
        if (collision.tag == "Player")
        {
            _onButton = true;
            _spriterenderer.sprite = onButton;
            PuzzleRoomManager.instance.ResetPasswordArrayIndex();
            SoundManager.instance.PlaySFX("Switch_2");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!_onButton)
            return;

        _onButton = false;
        _spriterenderer.sprite = nononButton;
    }
}
