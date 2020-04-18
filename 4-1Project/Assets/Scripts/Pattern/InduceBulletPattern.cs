using UnityEngine;
using System.Collections;

public class InduceBulletPattern : Pattern
{
    public InduceBulletPattern(PatternReceiver patternReceiver) : base(patternReceiver)
    {
    }

    public override void Execute()
    {
        patternReceiver.InduceBullet();
    }
}
