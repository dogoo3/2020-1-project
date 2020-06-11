using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginWindow : MonoBehaviour
{
    public GameObject LoginUI;

    private bool _isOnLoginUI;
    private void Awake()
    {
        SoundManager.instance.PlayBGM("Title_1");
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isOnLoginUI)
                TurnOffLoginUI();
        }
    }

    public void TurnOnLoginUI()
    {
        LoginUI.SetActive(true);
        _isOnLoginUI = true;
    }

    public void TurnOffLoginUI()
    {
        LoginUI.SetActive(false);
        _isOnLoginUI = false;
    }

    public void PlayClickSetting()
    {
        SoundManager.instance.PlaySFX("Fail_1");
    }

    public void PlayClickGameStart()
    {
        SoundManager.instance.PlaySFX("Start_1");
    }

    public void PlayClickExit()
    {
        SoundManager.instance.PlaySFX("Exit_1");
    }
    
    public void PlayClickButton()
    {
        SoundManager.instance.PlaySFX("Button_9");
    }
}
