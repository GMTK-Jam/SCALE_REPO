using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ranged : BaseEnemy
{
    public GameObject bullet;
    public float bulletSpeed;
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
        Debug.Log("Range attacking");
        Projectile projectile = Instantiate(bullet.gameObject,transform.position,Quaternion.identity).GetComponent<Projectile>();
        projectile.source = this;
        projectile.target = playerTarget;
        projectile.filter = damageContactFilter;
        projectile.speed = bulletSpeed;
        projectile.range = baseAttackDistance;
    }

    public void ProjectileHit(LimbClass damagedLimb)
    {
        base.RaiseDamageEvent(damagedLimb);
    }

}

