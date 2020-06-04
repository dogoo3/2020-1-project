using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropObject : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float alpha = 1.0f;
    private bool _isSpawnSwitch;
    private bool _isSuccessSwitch;
    private string _lastattackUsername;
    public GameObject dropObject;

    public bool tutorial;
    [Header("몇 대 맞으면 아이템을떨굴건지")]
    public int attackCount;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void MinusCount(string _nickname = "")
    {
        attackCount--;
        if(attackCount == 0) // 오브젝트 체력? 이 0이되면 
        {
            if(_nickname == GameManager.instance.PlayerName) // 클라 플레이어가 마지막으로 오브젝트를 때렸음
                _lastattackUsername = _nickname;
        }
    }

    private void Update()
    {
        if (attackCount <= 0)
        {
            spriteRenderer.color = new Color(1, 1, 1, alpha-=0.014f);
            if (alpha <= 0 && !tutorial)
            {
                if(dropObject != null)
                    dropObject.SetActive(true);
                gameObject.GetComponent<ItemDropObject>().enabled = false;
                Debug.Log(_isSpawnSwitch);
                if(_isSpawnSwitch && !_isSuccessSwitch)
                {
                    if (_lastattackUsername == GameManager.instance.PlayerName) // 마지막으로 때린 사람이 나라면
                        GameManager.instance._player.isGetSwitch = true; // 나는 더이상 스위치를 만들 수 없다.
                    ObjectPoolingManager.instance.GetQueue(ObjectPoolingManager.instance.queue_switch,transform.position + (Vector3.right*1)); // 풀링매니저에서 스위치 가져오고
                    _isSuccessSwitch = true; // 1개만 풀링될 수 있도록 함.
                }
            }
        }
    }

    public void ChangeSpawnSwitchState(bool _p_isState)
    {
        if(attackCount == 0) // 체력이 남은동안에만 스위치 생성가능 상태가 변경될 수 있도록 한다.
            _isSpawnSwitch = _p_isState;
    }

    public bool CheckCount()
    {
        if (attackCount == 0)
            return true;
        else
            return false;
    }
}
