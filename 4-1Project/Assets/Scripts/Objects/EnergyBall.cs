using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : MonoBehaviour
{
    Rigidbody2D _rigidbody2D;
    private float _time;
    private Vector2 _mousepos;

    public float lifetime = 2.0f;
    public int power;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        transform.position = Vector2.zero;
        _rigidbody2D.velocity = Vector2.zero;
    }

    private void Update()
    {
        _time += Time.deltaTime;
        if (_time > lifetime)
            ObjectPoolingManager.instance.InsertQueue(this,ObjectPoolingManager.instance.queue_energyball);
    }

    private void OnDisable()
    {
        _time = 0;
    }
}
