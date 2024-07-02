using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OrcBase : Enemy
{
    protected List<System.Action> attackPatterns = new List<System.Action>();
    protected Animator animator;
    private bool isIdle = true;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        InitializeAttackPatterns();
        StartCoroutine(PatrolBehaviour());
    }

    protected abstract void InitializeAttackPatterns();

    protected override void Update()
    {
        base.Update();
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        switch (currentState)
        {
            case MonsterState.Patrol:
                if (isIdle)
                {
                    animator.SetBool("isIdle", true);
                    animator.SetBool("isWalking", false);
                }
                else
                {
                    animator.SetBool("isIdle", false);
                    animator.SetBool("isWalking", true);
                }
                animator.SetBool("isChasing", false);
                animator.SetBool("isAttacking", false);
                break;
            case MonsterState.Chase:
                animator.SetBool("isIdle", false);
                animator.SetBool("isWalking", false);
                animator.SetBool("isChasing", true);
                animator.SetBool("isAttacking", false);
                break;
            case MonsterState.Attack:
                animator.SetBool("isIdle", false);
                animator.SetBool("isWalking", false);
                animator.SetBool("isChasing", false);
                animator.SetBool("isAttacking", true);
                break;
        }
    }

    private IEnumerator PatrolBehaviour()
    {
        while (true)
        {
            if(currentState == MonsterState.Patrol)
            {
                isIdle = !isIdle;
                yield return new WaitForSeconds(Random.Range(2f, 5f));
            }
            else
            {
                yield return null;
            }
        }
    }

    protected override void PerformAttack()
    {
        if(attackPatterns.Count > 0)
        {
            int randomIndex = Random.Range(0,attackPatterns.Count);
            attackPatterns[randomIndex]();
        }
    }
}
