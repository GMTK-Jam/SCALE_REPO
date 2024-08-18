using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Eye : BaseEnemy
{
    protected override void Start()
    {
        base.Start();
    }

    protected override float CalculateHealth()
    {
        return baseHealth + 4 * spawnWave;
    }

    protected override void PerformSkill()
    {
    }

}

