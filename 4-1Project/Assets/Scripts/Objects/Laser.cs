using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour
{
    private int _laserIndex;
    private float _time;
    private bool _isHit;
    
    private void Awake()
    {
    }

    private void OnEnable()
    {
        SetLaserIndex(_laserIndex);
    }

    private void Update()
    {
        _time += Time.deltaTime;
        if (_time >= 2.0f)
            ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_randomlaser);
        else
            transform.position = Boss.instance.transform.position;
        switch (_laserIndex)
        {
            case 0: // Down
                _isHit = Physics2D.Raycast(transform.position, Vector2.down);
                break;
            case 1: // DownRight
                _isHit = Physics2D.Raycast(transform.position, Vector2.down + Vector2.right);
                break;
            case 2: // Right
                _isHit = Physics2D.Raycast(transform.position, Vector2.right);
                break;
            case 3: // UpRight
                _isHit = Physics2D.Raycast(transform.position, Vector2.up + Vector2.right);
                break;
            case 4: // UP
                _isHit = Physics2D.Raycast(transform.position, Vector2.up);
                break;
            case 5: // UpLeft
                _isHit = Physics2D.Raycast(transform.position, Vector2.up + Vector2.left);
                break;
            case 6: // Left
                _isHit = Physics2D.Raycast(transform.position, Vector2.left);
                break;
            case 7: // DownLeft
                _isHit = Physics2D.Raycast(transform.position, Vector2.down + Vector2.left);
                break;
        }
    }

    private void OnDisable()
    {
        _time = 0;
    }

    public void SetLaserIndex(int _laserIndex)
    {
        this._laserIndex = _laserIndex;
        transform.rotation = Quaternion.Euler(0, 0, 45 * _laserIndex);
    }
}
