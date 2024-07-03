using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkeletonBase : Enemy
{
    protected List<System.Action> attackPatterns = new List<System.Action>();
    protected Animator animator;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        InitializeAttackPatterns();
    }

    protected abstract void InitializeAttackPatterns();

    protected override void Update()
    {
        base.Update();
        UpdateAnimationState();
    }

    protected virtual void UpdateAnimationState()
    {
        switch (currentState)
        {
            case MonsterState.Patrol:
                animator.SetBool("isChasing", false);
                animator.SetBool("isAttacking", false);
                break;
            case MonsterState.Chase:
                animator.SetBool("isChasing", true);
                animator.SetBool("isAttacking", false);
                break;
            case MonsterState.Attack:
                animator.SetBool("isChasing", false);
                animator.SetBool("isAttacking", true);
                break;
        }
    }

    protected override void PerformAttack()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            if (attackPatterns.Count > 0)
            {
                int randomIndex = Random.Range(0, attackPatterns.Count);
                attackPatterns[randomIndex]();
                lastAttackTime = Time.time;
            }
        }
    }
}
