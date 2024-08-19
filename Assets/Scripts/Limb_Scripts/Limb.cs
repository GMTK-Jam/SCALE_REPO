using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public abstract class Limb : MonoBehaviour
{
    public int stage { get; set; }
    public int xps { get; set; }
    public abstract int[] xpThresholds { get; }

    public abstract int[] stagesWeight { get; }


    public void AddXP(int addedXP)
    {
        xps += addedXP;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (stage < xpThresholds.Length && xps >= xpThresholds[stage])
        {
            LevelUp();
        }
    }

    public float FillPercentage()
    {
        if (stage == 0)
        {
            return ((float)xps / (float)xpThresholds[0]);
        }
        else if (stage >= xpThresholds.Length)
        {
            return 1f; // If at or beyond max stage, return 100% filled
        }
        else
        {
            return (((float)xps - (float)xpThresholds[stage - 1]) / ((float)xpThresholds[stage] - (float)xpThresholds[stage - 1]));
        }
    }

    public abstract void LevelUp();
}