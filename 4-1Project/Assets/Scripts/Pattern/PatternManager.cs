using UnityEngine;
using System.Collections;
using LitJson;

public class PatternManager : MonoBehaviour
{
    public static PatternManager instance;

    PatternCommand pat_induceBullet, pat_randomLaser;
    Vector2 playerPos;
    public PhaseEnd data_PhaseEnd;

    [HideInInspector] 
    public bool _isStart;
    private bool _isEnd;
    private int _index;

    private void Awake()
    {
        instance = this;

        data_PhaseEnd.Init();
    }

    private void Start()
    {
        pat_induceBullet = new InduceBullet();
        pat_randomLaser = new RandomLaser();
    }

    private void Update()
    {
        if (_isStart)
        {
            _isStart = false;
            switch (Boss.instance.patternNum)
            {
                case 2:
                    Debug.Log("총알 생성");
                    pat_induceBullet.Execute(Boss.instance._circleBullet);
                    break;
                case 3: // 유도 탄환
                    pat_induceBullet.Execute(playerPos);
                    Boss.instance.DelaySendPhaseData(0.5f);
                    break;
                case 4: // 랜덤 레이저
                    pat_randomLaser.Execute(_index);
                    Boss.instance.DelaySendPhaseData(0.5f);
                    break;

            }
            _isEnd = true;
        }

        if (_isEnd)
        {
            _isEnd = false;
            Invoke("SendPhaseEnd", 1.0f);
        }
    }

    public void LoadRandomLaser(JsonData _data) // 랜덤 레이저를 날릴 인덱스를 Resolve.
    {
        _isStart = true;
        _index = int.Parse(_data["laserDir"].ToString());
    }

    public void LoadInduceBullet(JsonData _data) // 유도 탄환을 날릴 플레이어의 위치를 Resolve.
    {
        _isStart = true;
        playerPos.x = float.Parse(_data["x"].ToString());
        playerPos.y = float.Parse(_data["y"].ToString());
    }

    public void PatternStart()
    {
        _isStart = true;
    }

    private void SendPhaseEnd()
    {
        CancelInvoke("SendPhaseEnd");
        Debug.Log("보스 패턴 재시작");
        JsonData SendData = JsonMapper.ToJson(data_PhaseEnd);
        ServerClient.instance.Send(SendData.ToString());
    }
}
