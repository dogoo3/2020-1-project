using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{
    public Image[] image_HPgauge; // 0번이 제일 위에 표시되도록
    public Text text_phase;
    public Text GameClearMessage;

    private float bossHP;
    private float bossFullHp;
    private float phaseFullHP; // 1페이즈 전체의 HP
    private float remainderHP; // 1페이즈에서 남은 HP

    public Color32[] HPGaugeColor;

    public int phase; // 초기에 정해지는 페이즈 변수
    private int carculatePhase; // 실제 연산에 들어가는 페이즈 변수

    private void OnEnable()
    {
        if (Boss.instance != null)
        {
            bossHP = Boss.instance.HP; // 보스의 현재 HP를 가져옴
            bossFullHp = Boss.instance._fullHp; // 보스의 전체 HP를 가져옴
            phase = Boss.instance.HPBarPhase;
            phaseFullHP = bossFullHp / phase; // 한 페이즈당 보스의 최대체력
        }

        carculatePhase = phase; // 페이즈 초기화
        remainderHP = phaseFullHP; // 페이즈HP 초기화
        image_HPgauge[0].color = HPGaugeColor[9];
        image_HPgauge[1].color = HPGaugeColor[8];
    }

    //private void OnEnable()
    //{
    //    phaseFullHP = bossHP / phase; // 한 페이즈당 보스의 최대체력

    //    remainderHP = phaseFullHP;
    //    image_HPgauge[0].color = HPGaugeColor[9];
    //    image_HPgauge[1].color = HPGaugeColor[8];
    //}

    private void Update()
    {
        bossHP = Boss.instance.HP;
        if (bossHP <= 0)
        {
            if(GameClearMessage != null)
                GameClearMessage.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        carculatePhase = (int)(bossHP / phaseFullHP); // 현재 보스 HP에서 페이즈당 최대HP를 나눠서 몇 페이즈인지 계산
        remainderHP = bossHP % phaseFullHP; //  페이즈에 남은 HP

        if (carculatePhase != 0)
            text_phase.text = "X" + carculatePhase.ToString();
        else
            text_phase.text = "";

        image_HPgauge[0].fillAmount = remainderHP / phaseFullHP;

        // 0번 페이즈는 투명페이즈.
        image_HPgauge[0].color = HPGaugeColor[Mathf.Clamp(carculatePhase + 1, 0, phase)]; // 최대값이 페이즈
        image_HPgauge[1].color = HPGaugeColor[Mathf.Clamp(carculatePhase, 0, phase - 1)]; // 최대값이 페이즈 - 1
    }

    private void Initialize()
    {
        if(Boss.instance != null)
            bossHP = Boss.instance.HP; // 보스의 총 HP를 가져옴
        phaseFullHP = bossHP / phase; // 한 페이즈당 보스의 최대체력

        remainderHP = phaseFullHP;
        image_HPgauge[0].color = HPGaugeColor[9];
        image_HPgauge[1].color = HPGaugeColor[8];
    }
}
