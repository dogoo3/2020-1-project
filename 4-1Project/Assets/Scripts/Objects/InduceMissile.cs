using UnityEngine;
using System.Collections;

public class InduceMissile : MonoBehaviour
{
    public Transform _induceObject;

    private bool _attacked;

    private float _cooltime = 1;
    private float _attackedTime;
    private float _lerpValue = 1.25f;

    private void OnEnable()
    {
        if (Boss.instance != null) // 처음 생성될 때는 보스가 준비되어있지 않으므로
        {
            transform.position = PatternManager.instance.transform.position;
            Invoke("InsertQueue", 5.0f);
        }
        if (_induceObject == null) // 목표물이 없을 경우
            ObjectPoolingManager.instance.InsertQueue(this);
    }

    private void Update()
    {
        if (_attacked)
        {
            if (Time.time - _attackedTime > _cooltime)
            {
                GameManager.instance._player.Attacked_Restriction(60);
                _attackedTime = Time.time;
            }
        }
        transform.position = Vector2.Lerp(transform.position, _induceObject.position, _lerpValue * Time.deltaTime);
        _lerpValue += 0.003f;
    }

    public void SetInduceObject(Transform _transform)
    {
        _induceObject = _transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == GameManager.instance.PlayerName)
            _attacked = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == GameManager.instance.PlayerName)
        {
            _attacked = false;
            GameManager.instance._player.Attacked(_attacked);
        }
    }

    #region Invoke
    private void InsertQueue()
    {
        ObjectPoolingManager.instance.InsertQueue(this);
        _lerpValue = 0.02f;
        PatternManager.instance.SendDelayPhaseEnd();
    }
    #endregion
}
