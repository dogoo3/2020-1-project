using UnityEngine;
using System.Collections;

public abstract class Pattern
{
    protected PatternReceiver patternReceiver;

    public Pattern(PatternReceiver patternReceiver)
    {
        this.patternReceiver = patternReceiver;
    }

    public abstract void Execute();
}
