using UnityEngine;
using System.Collections;
using LitJson;

public class PatternManager : MonoBehaviour
{
    public static PatternManager instance;
    
    PatternCommand pat_induceBullet, pat_randomLaser;
    Vector2 playerPos;
    public PhaseEnd data_PhaseEnd;

    private bool _isStart;
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
        if(_isStart)
        {
            switch (Boss.instance.patternNum)
            {
                case 3: // 유도 탄환
                    pat_induceBullet.Execute(playerPos);
                    break;
                case 4: // 랜덤 레이저
                    pat_randomLaser.Execute(_index);
                    break;
            }
            Invoke("SendPhaseEnd", 0.5f);
            _isStart = false;
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

    private void SendPhaseEnd()
    {
        CancelInvoke("SendPhaseEnd");
        JsonData SendData = JsonMapper.ToJson(data_PhaseEnd);
        ServerClient.instance.Send(SendData.ToString());
    }
}
