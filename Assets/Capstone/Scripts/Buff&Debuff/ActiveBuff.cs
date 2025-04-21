using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBuff
{
    public Buff buff;
    public float remainingTime;

    public ActiveBuff(Buff buff)
    {
        this.buff = buff;
        remainingTime = buff.duration;
    }
}
