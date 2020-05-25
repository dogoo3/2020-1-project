using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CineRoomCollider : MonoBehaviour
{
    private CinemachineConfiner _cineConfiner;

    public Collider2D[] roomAreaCollider;

    public static CineRoomCollider instance;

    private void Awake()
    {
        instance = this;
        _cineConfiner = GetComponent<CinemachineConfiner>();
    }

    public void SetAreaCollider(int _roonNum)
    {
        _cineConfiner.m_BoundingShape2D = roomAreaCollider[_roonNum - 1];
    }
}
