using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class PuzzleRoomManager : MonoBehaviour
{
    public static PuzzleRoomManager instance;

    private bool _isClear = false; // 퍼즐 완료 여부
    private int _passwordArrayIndex = 0; // 패스워드 배열 인덱스
    private int[] _password; // 밟은 마법진 번호를 저장하는 배열(패스워드)
    private List<string> _deadNickname; // 사망시킬 유저들의 닉네임

    public int[] correctPassword;
    public GameObject launchObject;
    public PuzzleroomStatue puzzleroomStatue; // 조각상 스크립트(퍼즐 맞추면 조각상이 깨지게 할려고)

    private void Awake()
    {
        instance = this;
        _password = new int[4];
        _deadNickname = new List<string>();
    }

    public void PlusMagicSqaureCount(int _magicSquareNum, string _myName) // 밟은 플레이어 수 1 증가
    {
        _password[_passwordArrayIndex++ % 4] = _magicSquareNum; // 패스워드 저장
        _deadNickname.Add(_myName); // 사망 대상 닉네임 등록
        CheckOnPlayerCount(); // 올라갔는데 2명 이상인지 판단.
    }

    public void MinusMagicSquareCount(string _myName) // 밟은 플레이어 수 1 감소
    {
        SearchPlayerNameDelete(_myName);
    }

    public void ResetPasswordArrayIndex()
    {
        _passwordArrayIndex = 0;
        for (int i = 0; i < _password.Length; i++)
            _password[i] = 0;
    }

    private void CheckOnPlayerCount()
    {
        if (_isClear)
            return;

        if(_deadNickname.Count > 1) // 2명 이상의 플레이어가 동시에 마법진을 밟으면
        {
            for (int i = 0; i < _deadNickname.Count; i++)
            {
                if (_deadNickname[i] == GameManager.instance.PlayerName) // 사망자 명단 중에 내 이름이 있을 경우
                {
                    GameManager.instance._player.Dead();
                    break;
                }
            }
            Debug.Log("2명 이상이 올라타 사망!");
        }
        else
        {
            for(int i=0;i<correctPassword.Length;i++)
            {
                if (correctPassword[i] != _password[i])
                    return;
            }
            puzzleroomStatue.DestroyStatue();
            Invoke("CorrectPassword", 1.0f);
            _isClear = true;
        }
    }

    private void SearchPlayerNameDelete(string _deleteName)
    {
        for (int i = 0; i < _deadNickname.Count; i++)
        {
            if (_deadNickname[i] == _deleteName)
            {
                _deadNickname.Remove(_deleteName);
                break;
            }
        }
    }

    private void CorrectPassword()
    {
        SoundManager.instance.PlaySFX("Boss_Portal_9");
        launchObject.SetActive(true);
    }
}

