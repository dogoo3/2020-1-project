using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ONOFFMixwindow : MonoBehaviour
{
    public GameObject gameobject;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == GameManager.instance.PlayerName)
            gameobject.SetActive(true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == GameManager.instance.PlayerName)
            gameobject.SetActive(false);   
    }
}
