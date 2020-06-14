using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCooltimeController : MonoBehaviour
{
    private Image cooltimePanel;

    private float _skillCooltime; // 스킬 쿨타임
    private float _divValue; // 1 / 스킬 쿨타임을 한 값.(곱셈연산을 위해 만듬)

    private bool _isLoadCool = false;

    private void Awake()
    {
        cooltimePanel = GetComponent<Image>();
    }

    public void SetCooltime(float _cooltime) // 쿨타임과 value값을 구함.
    {
        _skillCooltime = _cooltime; // 스킬 쿨만 바꿔놓고
        if(!_isLoadCool) // 스킬 쿨이 도는 중에는 쿨 UI 값이 바뀔 수 없다
            _divValue = 1 / _skillCooltime;
    }

    public void ShowCooltime(float _elapsedTime)
    {
        _isLoadCool = true;
        cooltimePanel.fillAmount = (_elapsedTime * _divValue - 1) * -1;
    }

    public void EndCooltime() // 0.000001의 쿨타임 오차를 없애기 위해 사용
    {
        cooltimePanel.fillAmount = 0;
        _isLoadCool = false;

        if (1 / _skillCooltime != _divValue) // 쿨 UI 값이 이전 쿨타임과 다를경우
            _divValue = 1 / +_skillCooltime; // 쿨 UI값을 재조정해준다
    }
}
