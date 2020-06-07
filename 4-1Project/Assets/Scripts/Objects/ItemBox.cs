using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class ItemBox : MonoBehaviour
{
    public static ItemBox instance;

    SpriteRenderer spriteRenderer;

    GetItemID _itemData;

    private int[] itemIDs = new int[3];
    private Vector2 _objectPos;
    private Vector2 _interval = new Vector2(5, 0);
    private int _getmoney;
    private bool _getitemid;

    public Sprite sprite_openItembox;

    public GetItem[] itemPrefabs;

    private void Awake()
    {
        instance = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
        _objectPos = transform.position;

        _itemData.Init();
    }

    private void Update()
    {
        if(_getitemid)
        {
            for (int i = -1; i < 2; i++)
                Instantiate(itemPrefabs[itemIDs[i+1]], _objectPos + _interval * i, Quaternion.identity);
            Inventory.instance.mymoney += _getmoney;
            Inventory.instance.text_money.text = Inventory.instance.mymoney.ToString();
            gameObject.GetComponent<ItemBox>().enabled = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == GameManager.instance.PlayerName) // 내가 박스를 터치했을 경우
        {
            spriteRenderer.sprite = sprite_openItembox;
            JsonData SendData = JsonMapper.ToJson(_itemData); // 박스에서 뜰 랜덤 아이템의 ID를 달라고 서버에 요청한다.
            ServerClient.instance.Send(SendData.ToString());
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
        else if (collision.tag == "Player") // 서버 플레이어가 박스를 터치했을 경우
        {
            spriteRenderer.sprite = sprite_openItembox;
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
        else { }
    }

    public void GetItemID(JsonData _data)
    {
        Debug.Log("보스사망 후 아이템 생성");
        for (int i = 0; i < 3; i++)
            itemIDs[i] = int.Parse(_data["itemIDs"][i].ToString());
        _getmoney = int.Parse(_data["money"].ToString());
        _getitemid = true;
    }
}
