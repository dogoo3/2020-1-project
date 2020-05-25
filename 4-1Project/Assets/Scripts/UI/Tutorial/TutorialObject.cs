using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObject : MonoBehaviour
{
    public GameObject magicianUI;
    public GameObject warriorUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == GameManager.instance.PlayerName)
        {
            if (GameManager.instance.type == 0) // 전사
                warriorUI.SetActive(true);
            else // 마법사
                magicianUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.name == GameManager.instance.PlayerName)
        {
            warriorUI.SetActive(false);
            magicianUI.SetActive(false);
        }
    }
}
