using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEventListener : CustomEventListener
{
    private Queue<DamageInfo> Damages;

    public void OnEventRaised(DamageInfo info)
    {
        Damages.Enqueue(info);
        base.OnEventRaised();
    }
    public DamageInfo PopDamage()
    {
        if (Damages.Count > 0)
        {
            return Damages.Dequeue();
        }
        else
        {
            return null;
        }
    }
}
