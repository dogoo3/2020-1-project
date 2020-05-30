using UnityEngine;

public class Restriction : MonoBehaviour
{
    private string _targetPlayername;

    private bool Attacked = false;

    private float cooltime = 1;
    private float attackedTime;

    private void OnEnable()
    {
        if(PatternManager.instance != null)
        {
            transform.parent = null;
            
            if(PatternManager.instance.restricTargetname == GameManager.instance.PlayerName)
            {
                Attacked = true;
                GameManager.instance._player.playerState = PlayerState.Restriction;
                transform.parent = GameManager.instance._player.transform;
                transform.localPosition = Vector2.zero;
            }
            else
            {
                Debug.Log(_targetPlayername);
                transform.parent = OtherPlayerManager.instance.PlayerList[_targetPlayername].transform;
                transform.localPosition = Vector2.zero;
            }
        }
    }

    private void Update()
    {
        if(Attacked)
        {
            if (Time.time - attackedTime > cooltime)
            {
                GameManager.instance._player.Attacked_Restriction(30);
                attackedTime = Time.time;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == _targetPlayername)
            return;
        else if (collision.tag == "Player")
        {
            Attacked = false;
            GameManager.instance._player.playerState = PlayerState.Idle;
            GameManager.instance._player.Attacked(false);
            ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_restriction);
            PatternManager.instance.SendDelayPhaseEnd();
        }
        else
        {

        }
    }

    public void SetTargetname(string _nickname)
    {
        _targetPlayername = _nickname;
        Debug.Log(_targetPlayername);
    }
}