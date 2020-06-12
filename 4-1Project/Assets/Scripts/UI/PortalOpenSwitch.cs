using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalOpenSwitch : MonoBehaviour
{
    private bool _isPush;
    private string _pushPlayername;
    private SpriteRenderer _spriteRenderer;

    public Sprite nonpushSwitch;
    public Sprite pushSwitch;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isPush) // 다른 플레이어가 누르고 있으면
            return; // 반환

        if(collision.tag == "Player")
        {
            _isPush = true; // 스위치가 눌러짐
            _pushPlayername = collision.name; // 스위치를 밟은 플레이어 이름 저장
            _spriteRenderer.sprite = pushSwitch; // 스위치 스프라이트 교체
            Debug.Log(_pushPlayername + "플레이어가 누름!");
            SoundManager.instance.PlaySFX("Switch_2");
            SwitchManager.instance.PlusSwitchCount();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!_isPush)
            return;

        if(collision.name == _pushPlayername) // 스위치를 누르고 있던 플레이어가 발을 뗄 경우
        {
            _isPush = false; // 스위치 해제
            _spriteRenderer.sprite = nonpushSwitch;
            SwitchManager.instance.MinusSwitchCount();
            Debug.Log(_pushPlayername + "플레이어가 발을 뗌!");
            // 스위치 매니저로 조절
        }
    }
}
