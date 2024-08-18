using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    public float cooldownTime;
    protected float nextUseTime;

    public BaseSkill(float cooldown)
    {
        cooldownTime = cooldown;
        nextUseTime = Time.time;
    }

    public bool CanUse()
    {
        return Time.time >= nextUseTime;
    }

    public void Use()
    {
        if (CanUse())
        {
            Perform();
            nextUseTime = Time.time + cooldownTime;
        }
    }

    protected abstract void Perform();
}

