using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropObject : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float alpha = 1.0f;
    public GameObject dropObject;

    [Header("몇 대 맞으면 아이템을떨굴건지")]
    public int attackCount;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void MinusCount()
    {
        attackCount--;
    }

    private void Update()
    {
        if (attackCount <= 0)
        {
            spriteRenderer.color = new Color(1, 1, 1, alpha-=0.014f);
            if (alpha <= 0)
            {
                if(dropObject != null)
                    dropObject.SetActive(true);
                gameObject.GetComponent<ItemDropObject>().enabled = false;
            }
        }
    }
}
