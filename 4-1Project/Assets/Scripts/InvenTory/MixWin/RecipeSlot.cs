using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RecipeSlot : MonoBehaviour, IPointerClickHandler
{
    private Image icon;

    public Image[] matSlotImages;
    public Text[] matSlotTexts;

    public MixMaterialSlot[] mixMaterialSlots;
    public Recipe recipe;

    private void Awake()
    {
        icon = GetComponent<Image>();
    }

    private void SetAlpha(float _alpha)
    {
        Color color = icon.color;
        color.a = _alpha;
        icon.color = color;
    }

    public void InitUI(float _alpha)
    {
        icon.sprite = recipe.icon;
        SetAlpha(_alpha);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 같은 레시피를 클릭하는 경우
        if (recipe.slotResultnum == MixWindow.instance.selectSlotNum)
            return;

        // 조합 슬롯에 이미 데이터가 있는 경우(다른 레시피가 클릭되어있는 경우.)
        if (mixMaterialSlots[0].item.itemID != 0)
        {
            // 조합슬롯에 들어가 있는 아이템을 다시 인벤토리에 반환시켜준다.
            for (int i = 0; i < mixMaterialSlots.Length; i++)
                mixMaterialSlots[i].BackupItem();
        }

        for(int i=0;i<matSlotImages.Length;i++)
            mixMaterialSlots[i].SetSlot(recipe.mat_icons[i], recipe.itemID[i], recipe.itemCount[i]);
        MixWindow.instance.text_money.text = Inventory.instance.mymoney + " / " + recipe.money;
        MixWindow.instance.selectSlotNum = recipe.slotResultnum;
    }
}
