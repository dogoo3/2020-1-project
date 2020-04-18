using UnityEngine;
using System.Collections;

public class PatternManager : MonoBehaviour
{
    public static PatternManager instance;

    PatternReceiver receiver = new PatternReceiver();
    Pattern randomLaser, induceBullet;

    private void Awake()
    {
        instance = null;
    }
    private void Start()
    {
        randomLaser = new RandomLaserPattern(receiver);
        induceBullet = new InduceBulletPattern(receiver);
    }

    public void LoadRandomLaser()
    {
        randomLaser.Execute();
    }

    public void LoadInduceBullet()
    {
        induceBullet.Execute();
    }
}
