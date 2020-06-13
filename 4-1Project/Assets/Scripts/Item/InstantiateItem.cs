using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateItem : MonoBehaviour
{
    Rigidbody2D _rigidbody2d;

    [Header("중력이 0이 되는 시간")]
    public float gravityTime;
    [Header("아이템이 뜨는 높이")]
    public int enableHigh;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _rigidbody2d.AddForce(Vector2.up * enableHigh);
        Invoke("Invoke_GravityTime", gravityTime);
    }

    private void Invoke_GravityTime()
    {
        _rigidbody2d.bodyType = RigidbodyType2D.Static;
        gameObject.GetComponent<Collider2D>().enabled = true;
    }
}
