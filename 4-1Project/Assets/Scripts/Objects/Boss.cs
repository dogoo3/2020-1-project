using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class Boss : MonoBehaviour
{
    public static Boss instance;

    Animator _animator;
    Rigidbody2D _rigidbody2D;
    Transform _playerPos;
    ShakeCamera _shakeCamera;

    [Header("보스 죽으면 오브젝트를 활성화할건가?")]
    public bool _isDeadActive;
    public ActiveBossDead _activeBossdead;

    public BossHPBar BossHPBar;

    private float _Nowpercent;

    public int HP, STR, DEF, HPBarPhase;
    [HideInInspector]
    public int _fullHp;

    public int patternNum;

    public Phase Data;

    public int _circleBullet;

    public int _floorDeathOn;   //장판 작동

    public Fire_Ball _fireBall; //불 구슬 데미지 관련

    bool _firstStart, _attack;

    private void Awake()
    {
        instance = this;
        _firstStart = true;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        BossHPBar = GameObject.Find("BossHPBar").transform.Find("BossHPBar").GetComponent<BossHPBar>();
        _animator = GetComponent<Animator>();
        _shakeCamera = FindObjectOfType<ShakeCamera>();
        _fullHp = HP;
        if (_isDeadActive)
            _activeBossdead = FindObjectOfType<ActiveBossDead>();
        Data.Init(false);
    }

    private void Start()
    {
        _playerPos = FindObjectOfType<Player>().transform;
    }

    public void SetHP(JsonData _data)
    {
        if (_fullHp == HP && _firstStart)
        {
            patternNum = int.Parse(_data["Phase"].ToString());
            _attack = true;
            PatternManager.instance._isStart = true;
            _firstStart = false;
        }

        HP = int.Parse(_data["Hp"].ToString());
    }
    private void Update()
    {
        if (_attack)
        {
            _animator.SetTrigger("Attack");
            _attack = false;
        }

        if (HP <= 0)
        {
            _animator.SetTrigger("Dead");
            if (PatternManager.instance.bossType == BossType.SemiBoss)
                SoundManager.instance.PlaySFX("Middle_B_Die_1");
            else
                SoundManager.instance.PlaySFX("Final_B_Die_9");
        }

    }

    public void Attack()
    {
        _attack = true;
    }

    public void ShakeCamera() // 애니메이션 이벤트 함수
    {
        _shakeCamera.PutShakeTime();
    }

    public void Dead()
    {
        if(_activeBossdead != null)
            _activeBossdead.ActiveObject();
        gameObject.SetActive(false);
    }

    public void SetFullHP()
    {
        HP = _fullHp;
    }

    public void SetPhase(JsonData _data)
    {
        patternNum = int.Parse(_data["Phase"].ToString());
    }

    public void DelaySendPhaseData(float _delayTime)
    {
        Invoke("SendPhaseData", _delayTime);
    }

    private void SendPhaseData()
    {
        CancelInvoke("SendPhaseData");
        Data.bx = transform.position.x;
        Data.by = transform.position.y;

        Data.px = _playerPos.transform.position.x;
        Data.py = _playerPos.transform.position.y;

        JsonData SendData = JsonMapper.ToJson(Data);
        ServerClient.instance.Send(SendData.ToString());
    }

    public void ActiveHPBar()
    {
        if (!BossHPBar.gameObject.activeSelf)
            BossHPBar.gameObject.SetActive(true);
    }

    public void SearchFireBall(JsonData _data)
    {
        _fireBall.ServerHitFireBall(_data["Name"].ToString());
    }
}