using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowObject : MonoBehaviour
{
    SpriteRenderer _renderer;
    Color32 color32;

    public int showSpeed = 2;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        color32 = _renderer.color;
    }

    private void Update()
    {
        if (color32.a <= 255)
        {
            color32.a = (byte)Mathf.Clamp(color32.a += 2, 0, 256);
            _renderer.color = color32;
            Debug.Log(color32.a);
            if (color32.a == 255)
                gameObject.GetComponent<ShowObject>().enabled = false; // 할 일 다 끝나면 스크립트 종료
        }
    }
}
