using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchManager : MonoBehaviour
{
    public static SwitchManager instance;

    private bool _isGet = false; // 퍼즐 완료 여부
    public int _pushSwitchPlayerCount = 0; // 스위치를 밟고 있는 플레이어 수
    private int _totalPlayerCount; // 총 플레이어 수 

    private void Awake()
    {
        instance = this;
        _totalPlayerCount = GameManager.instance.playerInfo.Count + 1; // 서버 + 플레이어(1)
    }

    public void PlusSwitchCount() // 밟은 플레이어 수 1 증가
    {
        _pushSwitchPlayerCount++;
        CheckTotalPlayerCount();
    }

    public void MinusSwitchCount() // 밟은 플레이어 수 1 감소
    {
        _pushSwitchPlayerCount--;
        CheckTotalPlayerCount();
    }

    private void CheckTotalPlayerCount()
    {
        if (_isGet) // 스위치 퍼즐을 풀었으면
            return; // 실행하지 않는다.

        if(_pushSwitchPlayerCount == _totalPlayerCount)
        {
            // 사운드 넣어주면 좋을 듯
            // 채팅창 시스템 메시지 넣어주고(빨간색, "새로운 레시피를 획득하였습니다!")
            Chatting.instance.PutSystemMessage("새로운 레시피를 획득했습니다!", "lime");
            // 레시피 넣어준다.
            MixWindow.instance.activeRecipe.Add(DataBase.instance.activeRecipeList[0]);
            _isGet = true;
            Debug.Log("조합 레시피 획득!");
        }
    }
}
