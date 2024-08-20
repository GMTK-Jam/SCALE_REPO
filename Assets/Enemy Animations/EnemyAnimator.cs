using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]

public class EnemyAnimator : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private EnemyAnimationState currState;
    private Animator _animator;
    private BaseEnemy _baseEnemyController;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _baseEnemyController = GetComponent<BaseEnemy>();
        currState = EnemyAnimationState.Default;

        _animator.SetBool("Death", false);
        _animator.SetBool("Attacking", false);
    }

    void Update()
    {
        
    }

    public void StartAttackAnimation()
    {
        if (currState == EnemyAnimationState.Default)
        {
            _animator.SetBool("Attacking",true);
            currState = EnemyAnimationState.Attack;
        }
        else
        {
            return;
        }
    }

    public void StartDefaultAnimation()
    {
        if (currState != EnemyAnimationState.Default)
        {
            _animator.SetTrigger("Default");
            _animator.ResetTrigger("Default");
            _animator.SetBool("Attacking", false);
            currState = EnemyAnimationState.Default;
        }
    }

    public void StartDeathAnimation()
    {
        if (currState != EnemyAnimationState.Death)
        {
            _animator.SetBool("Death",true);
            currState = EnemyAnimationState.Death;
        }
    }

    public void TakeDamage()
    {
        if (currState != EnemyAnimationState.Death)
        {
            _animator.SetTrigger("TakeDamage");
            _animator.ResetTrigger("TakeDamage");
        }
    }

    // Called by animation controller as it reaches hitting frames
    public void PerformedAttack()
    {
        if (currState != EnemyAnimationState.Attack)
        {
            return;
        }
        _baseEnemyController.SendMessage("PerformedAttack");
    }
}

public enum EnemyAnimationState
{
    Default,
    Skill,
    Attack,
    Death
}
