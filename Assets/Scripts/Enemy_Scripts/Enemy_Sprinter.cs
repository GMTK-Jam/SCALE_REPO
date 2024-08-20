using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Sprinter : BaseEnemy
{

    [Header("冲刺配置")]
    public float dashCooldown = 5f;
    public float dashDistance = 5f;
    public float dashDuration = 0.5f;
    [Tooltip("冲刺时避免的障碍物层")]
    public LayerMask obstacleLayerMask;
    private float nextDashTime;
    private bool isDashing;

    protected override void Start()
    {
        nextDashTime = Time.time;
        skills.Add(new Skill_Dash(aiPath, dashCooldown, dashDistance, dashDuration, obstacleLayerMask));
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (Time.time >= nextDashTime)
        {
            PerformDash();
            Debug.Log("Dash performed");
        }
    }

    protected override float CalculateHealth()
    {
        return baseHealth + 2 * spawnWave;
    }

    protected override void PerformSkill()
    {
    }

    private void PerformDash()
    {
        if (!isDashing)
            StartCoroutine(Dash());
        nextDashTime = Time.time + dashCooldown;
    }

    IEnumerator Dash()
    {
        StartDash();
        yield return new WaitForSeconds(1f);
        EndDash();
    }

    private void StartDash()
    {
        isDashing = true;
        aiPath.maxSpeed = baseMaxSpeed * 3;
    }

    private void EndDash()
    {
        aiPath.maxSpeed = baseMaxSpeed;
        isDashing = false;
    }
}

