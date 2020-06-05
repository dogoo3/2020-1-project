using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginWindow : MonoBehaviour
{
    public GameObject LoginUI;

    private bool _isOnLoginUI;

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
}
