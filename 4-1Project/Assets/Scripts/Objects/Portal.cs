using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform teleportPos;

    public Transform cameraPos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.transform.position = teleportPos.position;

            if (collision.name == GameManager.instance.PlayerName)
                Camera.main.transform.position = cameraPos.transform.position;
        }
    }
}
