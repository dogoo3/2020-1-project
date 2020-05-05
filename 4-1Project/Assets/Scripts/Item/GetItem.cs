using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetItem : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    private Animator animator;
    private bool isGetItem;
    
    public int itemID;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == GameManager.instance.PlayerName)
        {
            isGetItem = Inventory.instance.GetItem(itemID); // true를 반환하면 아이템을 획득한 것으로 판단.
            if (isGetItem)
                gameObject.SetActive(false);
        }
        else
            gameObject.SetActive(false);
    }

    public void DisableObject()
    {
        gameObject.SetActive(false);
    }

    public void EnableCollider()
    {
        boxCollider2D.enabled = true;
    }
    public void DisableCollider()
    {
        boxCollider2D.enabled = false;
    }
}
