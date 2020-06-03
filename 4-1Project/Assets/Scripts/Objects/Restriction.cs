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
            
            if(PatternManager.instance.restricTargetname == GameManager.instance.PlayerName) // 본인
            {
                Attacked = true;
                GameManager.instance._player.playerState = PlayerState.Restriction;
                GameManager.instance._player.Data.State = (int)PlayerState.Restriction;
                GameManager.instance._player.SendPlayerInfoPacket();
                
                transform.parent = GameManager.instance._player.transform;
                transform.localPosition = Vector2.zero;
            }
            else // 다른 유저
            {
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
            GameManager.instance._player.Data.State = (int)PlayerState.Idle;
            GameManager.instance._player.SendPlayerInfoPacket();
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
    }
}