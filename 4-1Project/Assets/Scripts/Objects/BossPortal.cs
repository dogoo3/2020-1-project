using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class BossPortal : MonoBehaviour
{
    [Header("보스방 전용포탈 스크립트. 키 없으면 못 들어감")]
    public Transform teleportPos;

    // public Transform cameraPos;

    public ShakeCamera shakecamera;
    [Header("이 포탈을 타고 이동하는 방 번호를 적으면 됨. 방번호는 1부터 시작함")]
    public int toMoveroomNum;

    public PlayerPortal data;

    private void Awake()
    {
        data.Init(GameManager.instance.PlayerName);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == GameManager.instance.PlayerName) // 내가 맞으면
        {
            if (Inventory.instance.CheckHaveKey()) // 키를 가지고 있으면
            {
                CineRoomCollider.instance.SetAreaCollider(toMoveroomNum); // 카메라 가두는 Confiner를 바꿔줌
                shakecamera.playerstateRoomnum = toMoveroomNum;
                GameManager.instance._player.StopDash(teleportPos); // 대시를 멈추고 최종목적지를 포탈타고 넘어가는 곳으로 변경.
                collision.gameObject.transform.position = teleportPos.position; // 플레이어를 이동

                data.x = teleportPos.position.x;
                data.y = teleportPos.position.y;
                data.get = true;
                JsonData SendData = JsonMapper.ToJson(data);
                ServerClient.instance.Send(SendData.ToString());
            }
            else
                Chatting.instance.PutSystemMessage("진입할 수 없습니다!", "red");
        }
    }
    
}
