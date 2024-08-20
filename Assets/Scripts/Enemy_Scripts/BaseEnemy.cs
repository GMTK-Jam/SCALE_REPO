using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using System.Runtime.CompilerServices;

[RequireComponent(typeof(EnemyAnimator))]
public abstract class BaseEnemy : MonoBehaviour
{
    public EnemyClass enemyClass;

    [Header("血量")]
    public float baseHealth;
    //[Header("最低移速")]
    //public float baseMinSpeed;
    [Header("攻击数值")]
    public float baseDamage;
    [Header("最大移速")]
    public float baseMaxSpeed;
    [Header("与目标保持距离")]
    public float baseStoppingDistance;
    [Header("攻击距离")]
    public float baseAttackDistance;
    [Header("攻击频次")]
    public float baseAttackInterval;
    [Header("当前波次（无需配置，取决于Spawner）")]
    public int spawnWave;
    [Header("特殊技能（无需配置，写在脚本里）")]
    public List<BaseSkill> skills = new List<BaseSkill>();
    public bool isRangedAttacker;

    protected Transform playerTarget;
    protected AIPath aiPath;

    [Tooltip("How long is blood pool visible after death")]
    [Range(1f, 5f)] public float deathStainTime = 2f;
    [Tooltip("How long does Enemy stop for after taking damage")]
    [Range(0f, 3f)] public float damageFreezeTime = 0f;

    private bool limbo = false;
    private bool frozen = false;
    private float attackTimer;
    private EnemyAnimator _enemyAnimator;
    public List<Collider2D> DamageColliders;
    public ContactFilter2D damageContactFilter;
    public CustomDamageEvent playerDamagedEvent;

    // ##############  Enemy Initialization ############## //

    protected virtual void Start()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
        if (playerTarget == null)
        {
            Debug.LogError("Player not found in the scene.");
            return;
        }
        _enemyAnimator = GetComponent<EnemyAnimator>();
        InitializeEnemy();
    }

    protected virtual void Update()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        TryAttack();
    }

    protected virtual float CalculateHealth()
    {
        return baseHealth;
    }

    protected virtual float CalculateAttackInterval()
    {
        return baseAttackInterval;
    }

    protected virtual void InitializeEnemy()
    {
        aiPath = GetComponent<AIPath>();
        aiPath.target = playerTarget;
        aiPath.endReachedDistance = baseStoppingDistance;
        aiPath.maxSpeed = baseMaxSpeed;
        baseHealth = CalculateHealth();
        baseAttackInterval = CalculateAttackInterval();
    }


    // ##############  Attacking ############## //

    protected virtual void TryAttack()
    {
        if (limbo || frozen)
        {
            return;
        }
        if (attackTimer > 0)
        {
            return;
        }
        if (!aiPath.reachedEndOfPath)
        {
            return;
        }
        StartAttack();
    }

    // Start an attack choreography
    protected virtual void StartAttack()
    {
        _enemyAnimator.StartAttackAnimation();
    }

    // Attack animation reached hitting frames, check for collision, reset timer;
    protected virtual void PerformedAttack()
    {
        attackTimer = baseAttackInterval;
        Blob damagedBlob = EvaluateAttackCollision();
        if (damagedBlob != null)
        {
            damagedBlob.EnemyHit();
            LimbClass damagedLimb = damagedBlob.limbMaster;
            RaiseDamageEvent(damagedLimb);
        }
    }

    protected virtual Blob EvaluateAttackCollision()
    {
        if (isRangedAttacker)
        {
            return null;
        }
        foreach (Collider2D collider in DamageColliders)
        {
            List<Collider2D> results = new List<Collider2D>();
            int overlaps = collider.OverlapCollider(damageContactFilter, results);

            if (overlaps > 0)
            {
                Blob damagedBlob = results[0].GetComponent<Blob>();
                return damagedBlob;
            }
        }
        return null;
    }

    protected virtual void RaiseDamageEvent(LimbClass damagedLimb)
    {
        DamageInfo damageInfo = new DamageInfo(baseDamage, enemyClass, damagedLimb);
        playerDamagedEvent.Raise(damageInfo);
    }



    protected virtual void PerformSkill()
    {
        if (limbo || frozen)
        {
            return;
        }

        // Randomly choose a skill that is off cooldown
        List<BaseSkill> availableSkills = skills.FindAll(skill => skill.CanUse());

        if (availableSkills.Count > 0)
        {
            int randomIndex = Random.Range(0, availableSkills.Count);
            availableSkills[randomIndex].Use();
        }
    }

    // ##############  Taking Damage ############## //

    // 受到伤害时可一定程度打断施法
    private IEnumerator DamageFreeze()
    {
        if (damageFreezeTime <= 0)
        {
            yield break;
        }
        frozen = true;
        aiPath.maxSpeed = 0;
        yield return new WaitForSeconds(damageFreezeTime);
        aiPath.maxSpeed = baseMaxSpeed;
        frozen = false;
    }
    public void TakeDamage(int damage)
    {
        _enemyAnimator.TakeDamage();
        baseHealth -= damage;
        if (baseHealth <= 0)
        {
            StartCoroutine(DeathSequence());
            limbo = true;
        }
        if (!frozen)
        {
            StartCoroutine(DamageFreeze());
        }
    }

    protected IEnumerator DeathSequence()
    {
        aiPath.maxSpeed = 0;
        _enemyAnimator.StartDeathAnimation();
        yield return new WaitForSeconds(deathStainTime);
        Destroy(gameObject, 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 10)
        {
            TakeDamage(5);
            Debug.Log("takedamage");
        }
    }



    // ############## Utilities ############## //

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, baseAttackDistance);
    }

}


public enum EnemyClass
{
    Heart,
    Lung,
    Arm,
    Leg,
    Mouth,
    Eye
}