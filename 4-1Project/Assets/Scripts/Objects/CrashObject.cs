using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashObject : MonoBehaviour
{
    private Player player;

    public int damage;

    private bool _isCrash;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == GameManager.instance.PlayerName)
        {
            if(player == null)
                player = collision.GetComponent<Player>();
            player.Attacked(damage);
            _isCrash = true;
        }
    }

    private void Update()
    {
        if (_isCrash)
            player.Attacked(damage);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _isCrash = false;
    }
}
