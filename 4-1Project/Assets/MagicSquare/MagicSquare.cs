using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSquare : MonoBehaviour
{
    private bool _onMagicsquare;
    private string _onPlayername;
    private SpriteRenderer _spriteRenderer;

    public Sprite nononMagicSquare;
    public Sprite onMagicSquare;

    [Header("패스워드 숫자 1개 입력")]
    public int passwordNumber;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_onMagicsquare) // 다른 플레이어가 누르고 있으면
            return; // 반환

        if (collision.tag == "Player")
        {
            _onMagicsquare = true; // 마법진이 눌러짐
            _onPlayername = collision.name; // 마법진을 밟은 플레이어의 이름 저장
            _spriteRenderer.sprite = onMagicSquare; // 마법진 스프라이트 교체
            PuzzleRoomManager.instance.PlusMagicSqaureCount(passwordNumber, collision.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!_onMagicsquare) // 다른 플레이어가 마법진 위에 있으면(2명 이상이 있다가 나올때)
            return;

        if(collision.name == _onPlayername)
        {
            _onMagicsquare = false; // 마법진에서 내려옴
            _spriteRenderer.sprite = nononMagicSquare;
            PuzzleRoomManager.instance.MinusMagicSquareCount(collision.name);
        }
    }
}
