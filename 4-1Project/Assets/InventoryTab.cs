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
    private Color _enableRGB, _disableRGB;

    [Header("0:장비 1:재료 2:소비")]
    public int _tabIndex;

    private void Awake()
    {
        _image = GetComponent<Image>();

        _enableRGB = new Color(0.545f, 0.545f, 0.545f, 1);
        _disableRGB = new Color(0.776f, 0.776f, 0.776f, 1);
    }

    public void EnableTab()
    {
        _image.color = _enableRGB;
    }

    public void DisableTab()
    {
        _image.color = _disableRGB;
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
