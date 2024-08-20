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
        Debug.Log("Blobperformed");
        base.PerformedAttack();
        StartCoroutine(DeathSequence());
    }

}

