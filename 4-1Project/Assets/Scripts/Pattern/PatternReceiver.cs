using UnityEngine;

public class PatternReceiver
{
    EnergyBall _energyBall;
    Laser _laserPattern;

    public void InduceBullet() // 유도 탄환
    {
        _energyBall = ObjectPoolingManager.instance.GetQueue(ObjectPoolingManager.instance.queue_energyball);
        if (_energyBall != null)
        {

        }
    }

    public void RandomLaser() // 랜덤 탄환
    {
        _laserPattern = ObjectPoolingManager.instance.GetQueue(ObjectPoolingManager.instance.queue_randomlaser);
        if(_laserPattern != null)
        {
            _laserPattern.SetLaserIndex();
            _laserPattern.gameObject.SetActive(true);
        }
    }
}