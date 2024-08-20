using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Blob : BaseEnemy
{

    private bool startedDeath;
    protected override void Start()
    {
        base.Start();
    }


    private void Update()
    {
        if(attacked == false)
        {
            EvaluateAttackCollision();

        }
        else
        {
            if (startedDeath == false)
            {
                StartCoroutine(DeathSequence());
                startedDeath = true;
            }

        }
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

