using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Magician : MonoBehaviour
{
    public static Player_Magician instance;

    private Player _mainPlayer;
    
    public float time, attacktime;

    private void Awake()
    {
        instance = this;

        _mainPlayer = GetComponent<Player>();
    }

    private void Update()
    {
        if (time < attacktime + 0.5f)
            time += 0.016f;

        if(Input.GetMouseButtonDown(0))
        {
            ObjectPoolingManager.instance.GetQueue();
        }
    }
}
