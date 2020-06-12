using UnityEngine;
using System.Collections;
public class GameEndSignal : MonoBehaviour
{
    public GameObject gameEndSignal;
    private bool _isIn;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isIn)
            return;
        if(collision.tag == "Player")
        {
            if(gameEndSignal != null)
                gameEndSignal.SetActive(true);
            GameManager.instance.Invoke("DeadCharacter", 5.0f);
            _isIn = true;
        }
    }
}
