using UnityEngine;

public class PatternCommand
{
    protected EnergyBall _energyball;
    protected Laser _laser;

    public virtual void Execute(int _index) { }
    public virtual void Execute(Vector2 _dir) { }
}

public class InduceBullet : PatternCommand
{
    public override void Execute(Vector2 _dir)
    {
        _energyball = ObjectPoolingManager.instance.GetQueue(ObjectPoolingManager.instance.queue_energyball);
        if (_energyball != null)
        {
            _energyball.InduceBullet(_dir);
        }
    }

    public override void Execute(int _index)
    {
        for (int i = 0; i < _index; ++i)
        {
            _energyball = ObjectPoolingManager.instance.GetQueue(ObjectPoolingManager.instance.queue_energyball);
            if (_energyball != null)
            {
                _energyball.InduceBullet(new Vector2(Mathf.Cos(Mathf.PI * 2 * i / _index), Mathf.Sin(Mathf.PI * 2 * i / _index)));
            }
        }
    }
}

public class RandomLaser : PatternCommand
{
    public override void Execute(int _laserIndex)
    {
        _laser = ObjectPoolingManager.instance.GetQueue(ObjectPoolingManager.instance.queue_randomlaser);
        if (_laser != null)
        {
            _laser.SetLaserIndex(_laserIndex);
            _laser.gameObject.SetActive(true);
        }
    }
}

