using UnityEngine;

public class PatternCommand
{
    protected Bullet _energyball;
    protected Laser _laser;

    public virtual void Execute(int _index) { }
    public virtual void Execute(Vector2 _dir) { }
    //총알을 위한 Execute(이름을 별도로 지어줘도 무방하다)
    public virtual void BulletExecute(int _index, BulletType type) { }
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

    public override void BulletExecute(int _index, BulletType _type)
    {
        float Theta = (Mathf.PI * 2) / _index;

        //홀수 탄환이면 홀수 탄환을 짝수면 짝수 탄환을 계산한다
        switch (_type)
        {
            case BulletType.EVEN_CIRCLE_NORMAL:
            case BulletType.EVEN_CIRCLE_CURVE:
                for (int i = 0; i < _index; ++i)
                {
                    _energyball = ObjectPoolingManager.instance.GetQueue(ObjectPoolingManager.instance.queue_energyball);
                    if (_energyball != null)
                    {
                        _energyball.InduceBullet(new Vector2(Mathf.Cos((Theta / 2.0f) + (Theta * i)), Mathf.Sin((Theta / 2.0f) + (Theta * i))), _type);
                    }
                }
                break;
            case BulletType.ODD_NUMBER_CIRCLE_NORMAL:
            case BulletType.ODD_NUMBER_CIRCLE_CURVE:
                for (int i = 0; i < _index; ++i)
                {
                    _energyball = ObjectPoolingManager.instance.GetQueue(ObjectPoolingManager.instance.queue_energyball);
                    if (_energyball != null)
                    {
                        _energyball.InduceBullet(new Vector2(Mathf.Cos(Theta * i), Mathf.Sin(Theta * i)), _type);
                    }
                }
                break;
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

