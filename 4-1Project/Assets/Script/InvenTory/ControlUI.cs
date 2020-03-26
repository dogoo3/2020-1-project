using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class ControlUI : MonoBehaviour
{
    public static ControlUI instance;

    public GameObject inventory;
    public InputField UI_typingfield;
    public Text UI_typingText;
    public Text UI_chattingLog;

    public SendMessage Data; // server Protocol

    private string _sendMessageBuffer;

    private void Awake()
    {
        instance = this;
        inventory.SetActive(true);
        inventory.SetActive(false);
    }

    private void Update()
    {
        if (!UI_typingfield.isFocused) // 채팅창이 활성화되어있는동안 다른 UI는 열고 닫을 수 없다.
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (inventory.activeSelf)
                    inventory.SetActive(false);
                else
                    inventory.SetActive(true);
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (UI_typingfield.text.Length > 0) // 채팅 보낼 때
            {
                Data.Init(GameManager.instance.PlayerName,UI_typingfield.text);
                JsonData SendData = JsonMapper.ToJson(Data);
                ServerClient.instance.Send(SendData.ToString());

                UI_chattingLog.text += GameManager.instance.PlayerName + " : " + UI_typingfield.text + "\n";
                UI_typingfield.text = "";
                UI_typingfield.ActivateInputField();

                
            }
            else // 캐럿 ON/OFF
            {
                if (UI_typingfield.gameObject.activeSelf)
                {
                    UI_typingfield.gameObject.SetActive(false);
                    return;
                }
                else
                {
                    UI_typingfield.gameObject.SetActive(true);
                    UI_typingfield.ActivateInputField();
                }
            }
        }
        if (_sendMessageBuffer != null) // 서버로부터 다른 클라이언트에서 들어온 메시지가 있는 경우
        {
            UI_chattingLog.text += _sendMessageBuffer; // 채팅로그에 업데이트해준 뒤
            _sendMessageBuffer = null; // 버퍼를 초기화해준다.
        }
    }

    public void ReceiveComment(JsonData _data)
    {
        _sendMessageBuffer += _data["username"].ToString() + " : " + _data["message"].ToString() + "\n";
    }
}
