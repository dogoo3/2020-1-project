using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

enum TabName
{
    Equipment,
    Material,
    Potion,
};

public class InventoryTab : MonoBehaviour, IPointerClickHandler
{
    private Image _image;

    public Sprite enableSprite, disableSprite;

    [Header("0:장비 1:재료 2:소비")]
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
        if (_tabIndex == (int)TabName.Equipment) // 장비
        {
            Inventory.instance.InitEquipInv(_tabIndex);
        }
        else if (_tabIndex == (int)TabName.Material) // 재료
        {
            Inventory.instance.InitMatInv(_tabIndex);
        }
        else if (_tabIndex == (int)TabName.Potion) // 소비
        {
            Inventory.instance.InitPotionInv(_tabIndex);
        }
        else { }
    }
}
