using UnityEngine;
using System.Collections;

public class RandomLaserPattern : Pattern
{
    public RandomLaserPattern(PatternReceiver patternReceiver) : base(patternReceiver)
    {
    }

    public override void Execute()
    {
        patternReceiver.RandomLaser();
    }
}
