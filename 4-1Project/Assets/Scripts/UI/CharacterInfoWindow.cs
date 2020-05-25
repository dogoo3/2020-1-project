using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoWindow : MonoBehaviour
{
    public static CharacterInfoWindow instance;

    [Header("0:HP, 1:ATK, 2:DEF, 3:ASPD, 4:SPD")]
    public Text[] text_Info;

    private float _playerFullHP;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _playerFullHP = HPManager.instance.myFullHP;
        UpdateHP(HPManager.instance.myHP);
    }

    public void UpdateHP(int _HP)
    {
        text_Info[0].text = _HP.ToString() + " / " + ((int)_playerFullHP).ToString();
    }

    public void UpdateATK(int _atk)
    {
        text_Info[1].text = _atk.ToString();
    }

    public void UpdateDEF(int _def)
    {
        text_Info[2].text = _def.ToString();
    }

    public void UpdateASPD(float _aspd)
    {
        text_Info[3].text = _aspd.ToString();
    }

    public void UpdateSPD(float _spd)
    {
        text_Info[4].text = _spd.ToString();
    }
}
