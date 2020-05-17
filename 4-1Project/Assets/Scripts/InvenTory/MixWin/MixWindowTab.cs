using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



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
        switch (_tabIndex)
        {
            case (int)MixWindowTabName.Armor:
                MixWindow.instance.ClickArmorTab();
                break;
            case (int)MixWindowTabName.Weapon:
                MixWindow.instance.ClickWeaponTab();
                break;
            case (int)MixWindowTabName.SubWeapon:
                MixWindow.instance.ClickSubWeaponTab();
                break;
            case (int)MixWindowTabName.Accessory:
                MixWindow.instance.ClickAccessoryTab();
                break;
            case (int)MixWindowTabName.Active:
                MixWindow.instance.ClickActiveTab();
                break;
        }
    }
}
