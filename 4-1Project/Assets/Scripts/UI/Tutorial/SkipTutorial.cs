using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipTutorial : MonoBehaviour
{
    public Transform inGameTransform;

    public int _playerCount;
    public int _nowInplayer;

    private bool _isFull;

    private void Awake()
    {
        _playerCount = GameManager.instance.playerInfo.Count + 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _nowInplayer++;
            if (_playerCount == _nowInplayer)
                _isFull = true;
        }
    }

    private void Update()
    {
        if (_isFull)
            Invoke("SkipTuto", 0.1f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _nowInplayer--;
    }

    private void SkipTuto()
    {
        GameManager.instance._player.transform.position = inGameTransform.position;
        for(int i=0;i< OtherPlayerManager.instance.PlayerList.Count;i++)
            OtherPlayerManager.instance.PlayerList[GameManager.instance.playerInfo[i].Name].transform.position = inGameTransform.position;
        CineRoomCollider.instance.SetAreaCollider(1);
        gameObject.SetActive(false);
    }
}
