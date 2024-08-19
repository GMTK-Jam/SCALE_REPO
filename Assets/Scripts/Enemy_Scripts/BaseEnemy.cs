using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
public abstract class BaseEnemy : MonoBehaviour
{
    [Header("血量")]
    public float baseHealth;
    //[Header("最低移速")]
    //public float baseMinSpeed;
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
    protected Transform playerTarget;
    protected AIPath aiPath;

    protected virtual void Start()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
        if (playerTarget == null)
        {
            Debug.LogError("Player not found in the scene.");
            return;
        }
        CalculateHealth();
        InitializeEnemy();
    }

    protected virtual void Update()
    {
    }

    protected virtual void InitializeEnemy()
    {
        aiPath = GetComponent<AIPath>();
        aiPath.target = playerTarget;
        aiPath.endReachedDistance = baseStoppingDistance;
        aiPath.maxSpeed = baseMaxSpeed;
        baseHealth = CalculateHealth();
    }

    protected virtual void PerformSkill()
    {
        // Randomly choose a skill that is off cooldown
        List<BaseSkill> availableSkills = skills.FindAll(skill => skill.CanUse());

        if (availableSkills.Count > 0)
        {
            int randomIndex = Random.Range(0, availableSkills.Count);
            availableSkills[randomIndex].Use();
        }
    }

    protected virtual float CalculateHealth()
    {
        return baseHealth;
    }
    public void TakeDamage(int damage)
    {
        baseHealth -= damage;
        if (baseHealth <= 0)
        {
            Die();
        }
    }

    protected void Die()
    {
        Destroy(gameObject, 0.1f);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, baseAttackDistance);
    }

}
