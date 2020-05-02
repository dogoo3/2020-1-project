using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

enum MixWindowTabName
{
    Armor,
    Weapon,
    SubWeapon,
    Accessory,
    Active,
};

public class MixWindowTab : MonoBehaviour, IPointerClickHandler
{
    private Image _image;

    public Sprite enableSprite, disableSprite;

    [Header("0:방어 1:무기 2:보조무기 3:액세서리 4:액티브")]
    public int _tabIndex;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void EnableTab()
    {
        _image.sprite = enableSprite;
    }

    public void DisableTab()
    {
        _image.sprite = disableSprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_tabIndex == (int)MixWindowTabName.Armor) // 방어구
        {
            MixWindow.instance.ClickArmorTab((int)MixWindowTabName.Armor);
        }
        else if (_tabIndex == (int)MixWindowTabName.Weapon) // 무기
        {
            MixWindow.instance.ClickWeaponTab((int)MixWindowTabName.Weapon);
        }
        else if (_tabIndex == (int)MixWindowTabName.SubWeapon) // 보조무기
        {
            MixWindow.instance.ClickSubWeaponTab((int)MixWindowTabName.SubWeapon);
        }
        else if (_tabIndex == (int)MixWindowTabName.Accessory) // 액세서리
        {
            MixWindow.instance.ClickAccessoryTab((int)MixWindowTabName.Accessory);
        }
        else if (_tabIndex == (int)MixWindowTabName.Active) // 액티브
        {
            MixWindow.instance.ClickActiveTab((int)MixWindowTabName.Active);
        }
        else { }
    }
}
