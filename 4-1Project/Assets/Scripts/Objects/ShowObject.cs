using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowObject : MonoBehaviour
{
    SpriteRenderer _renderer;
    Color color;

    public float showSpeed = 2;

    private void Awake()
    {
        showSpeed *= 0.003921568f;
        _renderer = GetComponent<SpriteRenderer>();
        color = _renderer.color;
    }

    private void Update()
    {
        if (color.a <= 1)
        {
            color.a = Mathf.Clamp(color.a += showSpeed, 0, 1);
            _renderer.color = color;
            Debug.Log(color.a);
            if (color.a == 1)
                gameObject.GetComponent<ShowObject>().enabled = false; // 할 일 다 끝나면 스크립트 종료
        }
    }
}
