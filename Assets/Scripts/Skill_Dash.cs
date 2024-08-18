using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class Skill_Dash : BaseSkill
{
    private AIPath aiPath;
    private float dashDistance;
    private float dashDuration;
    private LayerMask obstacleLayerMask;

    public Skill_Dash(AIPath path, float cooldown, float distance, float duration, LayerMask obstacleMask) : base(cooldown)
    {
        aiPath = path;
        dashDistance = distance;
        dashDuration = duration;
        obstacleLayerMask = obstacleMask;
    }

    protected override void Perform()
    {
        Vector3 dashDirection = (aiPath.destination - aiPath.transform.position).normalized;
        
        // Check if there are obstacles in the dash path
        if (CanDash(dashDirection))
        {
            aiPath.StartCoroutine(DashOverTime(dashDirection));
        }
    }

    private bool CanDash(Vector3 direction)
    {
        // Dash only if there are no obstacles in the path
        RaycastHit2D hit = Physics2D.Raycast(aiPath.transform.position, direction, dashDistance, obstacleLayerMask);
        return hit.collider == null;  
    }

    private IEnumerator DashOverTime(Vector3 direction)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = aiPath.transform.position;

        while (elapsedTime < dashDuration)
        {
            aiPath.transform.position = Vector3.Lerp(startPosition, startPosition + direction * dashDistance, elapsedTime / dashDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        aiPath.transform.position = startPosition + direction * dashDistance;
    }
}
