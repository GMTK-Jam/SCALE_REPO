using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DamageEvent")]
public class CustomDamageEvent : CustomEvent
{
    public void Raise(DamageInfo info)
    {
        foreach (CustomEventListener listener in listeners)
        {
            listener.OnEventRaised();
        }
    }
}
