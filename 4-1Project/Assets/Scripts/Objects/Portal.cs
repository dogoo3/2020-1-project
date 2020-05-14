using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform teleportPos;

    public Transform cameraPos;

    public ShakeCamera shakecamera;
    [Header("이 포탈을 타고 이동하는 방 번호를 적으면 됨. 방번호는 1부터 시작함")]
    public int toMoveroomNum;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.transform.position = teleportPos.position;

            if (collision.name == GameManager.instance.PlayerName) // 내가 이동하면
            {
                CineRoomCollider.instance.SetAreaCollider(toMoveroomNum); // 카메라 가두는 Confiner를 바꿔줌
                shakecamera.playerstateRoomnum = toMoveroomNum;
                GameManager.instance._player.StopDash(teleportPos); // 대시를 멈추고 최종목적지를 포탈타고 넘어가는 곳으로 변경.
            }
        }
    }
}
