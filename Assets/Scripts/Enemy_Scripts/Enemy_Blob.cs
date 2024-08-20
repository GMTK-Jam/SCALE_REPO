using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Blob : BaseEnemy
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

    public override void PerformedAttack()
    {
        base.PerformedAttack();
        StartCoroutine(DeathSequence());
    }

}

