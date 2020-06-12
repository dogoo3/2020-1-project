using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class TempScript : MonoBehaviour
{
    public Playerexit gameOver;

    private void Awake()
    {
        gameOver.Init();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.name == GameManager.instance.PlayerName)
            {
                Debug.Log("Send");

                JsonData SendData = JsonMapper.ToJson(gameOver);
                ServerClient.instance.Send(SendData.ToString());

                GameManager.instance.DeadCharacter();

            }
            else
            {
                collision.gameObject.SetActive(false);
            }
        }
    }
}
