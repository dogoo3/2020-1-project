using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum PlayerState
{
    Idle,
    Move,
    Dash,
    Attack,
    Skill,
    Invincible,
    Meteor,
    Restriction,
    Die,
};

public class Player : MonoBehaviour
{
    public PlayerState playerState;

    private List<RaycastResult> results = new List<RaycastResult>(); // 여기에 히트 된 개체 저장
    private GraphicRaycaster gr;
    private PointerEventData ped;
    private GameObject _InvUI;

    Animator _animator;
    
    private SubAnimator[] _subAnimator;
    [HideInInspector]
    public SkillCooltimeController _cooltimecontroller;

    [HideInInspector]
    public Vector2 _mousePos;
    [HideInInspector]
    public ItemDropObject temp;

    Vector2 _dirPos, _temp_dirPos;
    Vector2 _Pos, _movePos, _temp_movePos;

    public PlayerData Data;
    public BossDamage PtoB_damage_data; // 플레이어가 보스에게 데미지를 넣을 때
    public PlayerDamage BtoP_damage_data; // 보스가 쏜 탄환에 플레이어가 맞으면
    public ItemPerResult itemperresult_data;
    public Playerexit gameOver;
    public ParticleSystem[] dashBlur;

    public float _movespeed = 5.0f, invincibleTime, dashDelay = 2.0f;
    [HideInInspector]
    public float rate_attackspeed, rate_cooltime; // 누적 공속/쿨타임
    public float attackcooltime, skillcooltime; // 현재 공속/쿨타임 -> 공속은 반비례
    private float _ani_attackspeed = 1; // 애니메이션 재생속도. 곱셈 정비례
    private float _ori_attackCooltime, _ori_skillcooltime; // 첫 공속, 쿨타임

    public int STR, DEF;
    
    public bool _isCrash, isGetSwitch, isSwitch, isSetSwitch;
    private float time;

    private Vector2 toPos;
    private float timeStartdash;
    private bool dash;
    private float dashCooltime;
    private bool firstButtonPressed;
    private float timeOfFirstButton;
    private float dashSpeed = 0.2f;
    private float ori_dashSpeed;
    private bool _isDead;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _subAnimator = GetComponentsInChildren<SubAnimator>();
        gr = FindObjectOfType<GraphicRaycaster>();
        ped = new PointerEventData(null);

        _cooltimecontroller = GameObject.Find("SkillIcon").transform.Find("CooltimePanel").GetComponent<SkillCooltimeController>();

    }
    void Start()
    {
        playerState = PlayerState.Idle;
        _temp_dirPos = Vector2.zero;
        _temp_movePos = Vector2.zero;

        Data.Init(GameManager.instance.PlayerName);
        Data.Speed = _movespeed;

        PtoB_damage_data.Init();
        BtoP_damage_data.Init();
        itemperresult_data.Init();
        gameOver.Init();
        BtoP_damage_data.nickname = GameManager.instance.PlayerName;
        ori_dashSpeed = dashSpeed;
        _ori_attackCooltime = attackcooltime;
        _ori_skillcooltime = skillcooltime;
        CharacterInfoWindow.instance.UpdateATK(STR);
        CharacterInfoWindow.instance.UpdateDEF(DEF);
        CharacterInfoWindow.instance.UpdateSPD(_movespeed);
        _cooltimecontroller.SetCooltime(skillcooltime);
    }

    public void Update()
    {
        if(playerState == PlayerState.Die)
        {
            if (!_isDead)
            {
                if (GameManager.instance.type == 0)
                    SoundManager.instance.PlaySFX("1PC_Die_2");
                else
                    SoundManager.instance.PlaySFX("2PC_Die_2");
                Invoke("Invoke_Dead",4.0f);
                _isDead = true;
            }
            return;
        }
        if(playerState == PlayerState.Restriction)
        {
            if(dash) // 대시중에 속박먹으면 대시 강제종료
            {
                dashSpeed = ori_dashSpeed; // 첫 대시 스피드로 바꿔줌.
                ShowDashBlur(false);
                dash = false;
            }
            return;
        }
        // 이동뿐만이 아니라 회전했을 때도 현재 위치를 패킷으로 보내주기 때문에
        // (패킷을 보낼 때 현재 위치도 계속 보내기 때문에 최신 위치 정보가 필요해서)
        if (playerState != PlayerState.Die && playerState != PlayerState.Dash &&
            playerState != PlayerState.Meteor && playerState != PlayerState.Restriction) // 사망상태이거나 대시중, 법사 메테오 사용중이 아닐경우
        {
            // 현재 위치
            Data.nx = transform.position.x;
            Data.ny = transform.position.y;
            // 시점 변환
            ChangeLookdirection();
            // 캐릭터 이동
            MoveCharacter();
            // 대시 키 입력
            DashCharacter();
        }

        CarculateDashTime(); // 대시 시간을 계산, 대시 쿨타임이랑 대시목적지 도달 시간이 다르기 때문에 조건을 걸 수 없음.

        if(_isCrash) // 피격당했을 때 무적시간을 계산해서 무적을 풀어준다.
        {
            time += Time.deltaTime;
            if(time > invincibleTime)
            {
                time = 0;
                _isCrash = false;
                ChangeAnimationState_Attacked(_isCrash);
            }
        }
    }

    public void SetPercentItem(JsonData Data)
    {
        isSwitch = bool.Parse(Data["result"].ToString());
        Debug.Log(isSwitch);
        if (temp != null)
            temp.ChangeSpawnSwitchState(isSwitch); // 클라이언트에서 스위치 생성을 랜덤한 bool값을 받아오기 때문에 그 캐릭터와 똑같은 값을 가지고 있다.
        //isSetSwitch = true;
    }

    public void SendPlayerInfoPacket()
    {
        JsonData SendData = JsonMapper.ToJson(Data);
        ServerClient.instance.Send(SendData.ToString());
    }

    public void SendItemPercentPacket()
    {
        JsonData SendData = JsonMapper.ToJson(itemperresult_data);
        ServerClient.instance.Send(SendData.ToString());
    }

    public void ChangeLookdirection()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePos -= (Vector2)transform.position;
        _mousePos.Normalize();

        // 마우스 커서의 좌표를 -1 ~ 1로 만들어줌
        _dirPos.x = Mathf.RoundToInt(_mousePos.x);
        _dirPos.y = Mathf.RoundToInt(_mousePos.y);
        

        // 마우스 좌표에 따른 캐릭터의 시점 변경
        _animator.SetFloat("xPos", _dirPos.x);
        _animator.SetFloat("yPos", _dirPos.y);

        Data.rx = _dirPos.x;
        Data.ry = _dirPos.y;

        if (_temp_dirPos != _dirPos)
        {
            _temp_dirPos = _dirPos;
            JsonData SendData = JsonMapper.ToJson(Data);
            ServerClient.instance.Send(SendData.ToString());
        }
    }

    public void MoveCharacter()
    {
        _movePos.x = Input.GetAxisRaw("Horizontal");
        _movePos.y = Input.GetAxisRaw("Vertical");

        // 방향(키보드)
        Data.x = _movePos.x;
        Data.y = _movePos.y;

        Data.State = (int)PlayerState.Move;

        if (_movePos != Vector2.zero)
        {
            ChangeAnimationState_Move(true);
            transform.Translate(_movePos.normalized * Time.deltaTime * _movespeed);
            if (GameManager.instance.type == 0)
                SoundManager.instance.PlaySFX("1PC_Move_3",false);
            else
                SoundManager.instance.PlaySFX("2PC_Move_1",false);
        }
        else
        {
            Data.State = (int)PlayerState.Idle;
            ChangeAnimationState_Move(false);

            if (GameManager.instance.type == 0)
                SoundManager.instance.StopSFX("1PC_Move_3");
            else
                SoundManager.instance.StopSFX("2PC_Move_1");
        }

        if(_temp_movePos != _movePos)
        {
            _temp_movePos = _movePos;
            JsonData SendData = JsonMapper.ToJson(Data);
            ServerClient.instance.Send(SendData.ToString());
        }
    }

    public void DashCharacter() // 플레이어의 대시를 컨트롤함.
    {
        if (Input.GetKeyDown(KeyCode.Space) && firstButtonPressed) // 두 번째 클릭(대시중일때는 중복대시할 수 없다)
        {
            if (Time.time - timeOfFirstButton < 0.5f) // 0.5초 이내로 더블클릭하면
            {
                toPos = (Vector2)transform.position + _mousePos * 8.0f; // 대시할 좌표값목적지 쏴주고
                timeStartdash = Time.time; // 대시 시작 시간 저장
                playerState = PlayerState.Dash; // 상태변경
                // 서버로 관련 데이터 전송
                Data.dx = toPos.x;
                Data.dy = toPos.y;
                Data.State = (int)PlayerState.Dash;
                ShowDashBlur(true);
                dash = true; // 대시!
                dashCooltime = Time.time;
                if (GameManager.instance.type == 0) // 전사플레이어
                    SoundManager.instance.PlaySFX("1PC_Dash_1");
                else
                    SoundManager.instance.PlaySFX("2PC_Dash_1");

                JsonData SendData = JsonMapper.ToJson(Data);
                ServerClient.instance.Send(SendData.ToString());
            }
            firstButtonPressed = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !firstButtonPressed && Time.time - dashCooltime > 0.5f) // 첫 번째 클릭
        {
            firstButtonPressed = true; // 두 번째 클릭 가능
            timeOfFirstButton = Time.time; // 첫 번째 클릭 시간 저장
        }
    }

    public void CarculateDashTime() // 플레이어가 대시하는 실제 움직임.
    {
        if (dash)
        {
            transform.position = Vector2.Lerp(transform.position, toPos, dashSpeed); // 대시!
            dashSpeed += 0.03f;
            if (Mathf.Approximately(transform.position.x, toPos.x) && // 대시후 목적지까지 도달했을 경우
               Mathf.Approximately(transform.position.y, toPos.y))
                StopDash(transform);
        }
    }

    public void StopDash(Transform to_transform)
    {
        playerState = PlayerState.Idle; // 상태를 아이들로 딱 한 번 바꿔줌.
        Data.State = (int)PlayerState.Idle;
        Data.nx = to_transform.position.x;
        Data.ny = to_transform.position.y;
        dashSpeed = ori_dashSpeed; // 첫 대시 스피드로 바꿔줌.
        ShowDashBlur(false);
        dash = false;

        if (GameManager.instance.type == 0) // 전사플레이어
            SoundManager.instance.StopSFX("1PC_Dash_1");
        else
            SoundManager.instance.StopSFX("2PC_Dash_1");

        JsonData SendData = JsonMapper.ToJson(Data);
        ServerClient.instance.Send(SendData.ToString());
    }
    
    void ChangeAnimationState_Move(bool _state) // 걷기
    {
        for (int i = 0; i < _subAnimator.Length; i++)
        {
            if (_subAnimator[i].active)
            {
                _subAnimator[i].Move(_state);
                break;
            }
        }
    }

    void ShowDashBlur(bool _isStart) // 대시 표현
    {
        for (int i = 0; i < _subAnimator.Length; i++)
        {
            if (_subAnimator[i].active)
            {
                if(_isStart)
                    dashBlur[i].Play();
                else
                    dashBlur[i].Stop();
                break;
            }
        }
    } 

    public void ChangeAnimationState_Attack() // 공격
    {
        for (int i = 0; i < _subAnimator.Length; i++)
            _subAnimator[i].Attack();
    }

    public void ChangeAnimationState_Meteor()
    {
        for (int i = 0; i < _subAnimator.Length; i++)
            _subAnimator[i].Meteor();
    }

    public void ChangeAnimationState_Dead()
    {
        for (int i = 0; i < _subAnimator.Length; i++)
            _subAnimator[i].Dead();
    }

    public void ChangeAnimationState_Attacked(bool _isAttacked) // 피격당했을때 애니메이션, true면 피격중, false면 피격해제.
    {
        for (int i = 0; i < _subAnimator.Length; i++)
            _subAnimator[i].Attacked(_isAttacked);
    }

    public void ChangeAnimationValue_AttackSpeed(float _value)
    {
        for (int i = 0; i < _subAnimator.Length; i++)
            _subAnimator[i].SetAttackSpeed(_value);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<CapsuleCollider2D>(), collision.collider);
        }
    }

    public void AttackPlayer(PlayerState _attackstate = PlayerState.Attack) // 플레이어가 다른 무언가를 공격
    {
        playerState = _attackstate; // 상태변경
        Data.State = (int)_attackstate; // 서버로 보낼 패킷의 상태변경
        Data.ax = _mousePos.x; // 서버로 보낼 패킷의 마우스 정규화 좌표
        Data.ay = _mousePos.y; 
        ChangeAnimationState_Attack(); // 공격 애니메이션 작동
        JsonData SendData = JsonMapper.ToJson(Data);
        ServerClient.instance.Send(SendData.ToString());
    }

    public void SendDamageInfo(int _def) // 내가 공격을 할 때
    {
        if (STR - _def <= 0)
            return;
        PtoB_damage_data.damage = STR - _def;
        JsonData SendData = JsonMapper.ToJson(PtoB_damage_data);
        ServerClient.instance.Send(SendData.ToString());
    }

    public void Attacked(int _damage) // 외부에서 공격이 들어올 때.
    {
        if (_damage <= DEF) // 방어력이 들어온 데미지보다 높을 경우
            return;
        if (_isCrash) // 무적 상태일 경우(애니메이션을 위한 BOOL)
            return; // 피격 취소
        if (playerState == PlayerState.Invincible) // PlayerState에서 무적상태
            return;

        _isCrash = true;
        ChangeAnimationState_Attacked(_isCrash);
        HPManager.instance.myHP = (int)Mathf.Clamp(HPManager.instance.myHP - (_damage - DEF), -1, HPManager.instance.myFullHP);
        HPManager.instance.SetHP();
        BtoP_damage_data.HP = HPManager.instance.myHP;

        if (GameManager.instance.type == 0) // 전사플레이어
            SoundManager.instance.PlaySFX("1PC_Hit_4");
        else
            SoundManager.instance.PlaySFX("2PC_Hit_4");

        JsonData SendData = JsonMapper.ToJson(BtoP_damage_data);
        ServerClient.instance.Send(SendData.ToString());
        
        if (HPManager.instance.myHP <= 0) // 공격 당했는데 피가 없으면
        {
            playerState = PlayerState.Die;
            Data.State = (int)PlayerState.Die;
            ChangeAnimationState_Dead();
            SendPlayerInfoPacket();
        }
    }

    public void Attacked_Restriction(int _secDamage) // 속박과 유도미사일에 맞을 때마다 체력 감소
    {
        ChangeAnimationState_Attacked(true);
        HPManager.instance.myHP -= _secDamage;
        HPManager.instance.SetHP();
        BtoP_damage_data.HP = HPManager.instance.myHP;

        JsonData SendData = JsonMapper.ToJson(BtoP_damage_data);
        ServerClient.instance.Send(SendData.ToString());

        if (HPManager.instance.myHP <= 0) // 속박 당했는데 피가 없으면
        {
            playerState = PlayerState.Die;
            Data.State = (int)PlayerState.Die;
            ChangeAnimationState_Dead();
            SendPlayerInfoPacket();
        }
    }

    public void Dead()
    {
        HPManager.instance.myHP = 0;
        HPManager.instance.SetHP();
        BtoP_damage_data.HP = 0;

        JsonData SendData = JsonMapper.ToJson(BtoP_damage_data);
        ServerClient.instance.Send(SendData.ToString());
    }
    
    public bool CheckClickInv()
    {
        bool _is = false;

        results.Clear();
        ped.position = Input.mousePosition;
        gr.Raycast(ped, results);
        
        for (int i = 0; i < results.Count; i++)
        {
            _InvUI = results[i].gameObject;
            if (_InvUI.tag == "Inventory")
                _is =  true;
            else
                _is =  false;
        }
        return _is;
    }

    public void SetPlayerAvality()
    {
        ChangeAnimationValue_AttackSpeed(_ani_attackspeed + rate_attackspeed); // 애니메이션 실행속도 조정(정비례)
        attackcooltime = _ori_attackCooltime - rate_attackspeed; // 공격 쿨타임 조정(반비례)

        _cooltimecontroller.SetCooltime(_ori_skillcooltime * (1f - rate_cooltime));
        Data.attackspeed = _ani_attackspeed + rate_attackspeed;
        SendPlayerInfoPacket();
    }

    #region SimpleFunc
    public void ChangePS(PlayerState _ps)
    {
        playerState = _ps;
        Data.State = (int)_ps;
    }
    #endregion
    #region Invoke
    public void Invoke_ChangePSIdle() // 아이들로 상태변경(무적->, 메테오->)
    {
        playerState = PlayerState.Idle;
        Data.State = (int)PlayerState.Idle;
    }
    public void Invoke_DivideDEF() // 방어력 절반으로 감소
    {
        DEF /= 2;
        CharacterInfoWindow.instance.UpdateDEF(DEF);
    }
    public void Invoke_Dead() // 플레이어 사망
    {
        JsonData SendData = JsonMapper.ToJson(gameOver);
        ServerClient.instance.Send(SendData.ToString());

        GameManager.instance.DeadCharacter();
    }
    #endregion
}
