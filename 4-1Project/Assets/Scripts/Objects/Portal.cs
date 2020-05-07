using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform teleportPos;

    public Transform cameraPos;

    [Header("이 포탈을 타고 이동하는 방 번호를 적으면 됨. 방번호는 1부터 시작함")]
    public int toMoveroomNum;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.transform.position = teleportPos.position;

            if (collision.name == GameManager.instance.PlayerName)
                CineRoomCollider.instance.SetAreaCollider(toMoveroomNum);
                //Camera.main.transform.position = cameraPos.transform.position;
        }
    }
}
