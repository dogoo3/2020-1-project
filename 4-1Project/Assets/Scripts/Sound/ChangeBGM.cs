using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBGM : MonoBehaviour
{
    [Header("플레이어가 이 오브젝트로 가면 BGM이 변경됩니다.")]
    public string _bgmName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == GameManager.instance.PlayerName)
            SoundManager.instance.PlayBGM(_bgmName);
    }
}
