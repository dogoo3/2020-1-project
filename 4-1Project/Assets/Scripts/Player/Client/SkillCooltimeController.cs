using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCooltimeController : MonoBehaviour
{
    private Image cooltimePanel;

    private float _skillCooltime; // 스킬 쿨타임
    private float _divValue; // 1 / 스킬 쿨타임을 한 값.(곱셈연산을 위해 만듬)

    private void Awake()
    {
        cooltimePanel = GetComponent<Image>();
    }

    public void SetCooltime(float _cooltime) // 쿨타임과 value값을 구함.
    {
        _skillCooltime = _cooltime;
        _divValue = 1 / _skillCooltime;
    }

    public void ShowCooltime(float _elapsedTime)
    {
        cooltimePanel.fillAmount = (_elapsedTime * _divValue - 1) * -1;
    }

    public void EndCooltime() // 0.000001의 쿨타임 오차를 없애기 위해 사용
    {
        cooltimePanel.fillAmount = 0;
    }
}
