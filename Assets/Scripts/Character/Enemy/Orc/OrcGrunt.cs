using System.Collections;
using UnityEngine;
using static SkillData;

public class OrcGrunt : OrcBase
{
    private bool isAttacking = false;

    protected override void InitializeAttackPatterns()
    {
        attackPatterns.Add(() => StartCoroutine(PerformAttack(MeleeAttackV1)));
        attackPatterns.Add(() => StartCoroutine(PerformAttack(MeleeAttackV2)));
    }

    private IEnumerator PerformAttack(System.Action attack)
    {
        if (isAttacking) yield break;

        isAttacking = true;
        currentState = MonsterState.Attack;
        attack.Invoke();
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        currentState = MonsterState.Patrol;
        UpdateAnimationState();
        isAttacking = false;
    }

    private void MeleeAttackV1()
    {
        Debug.Log("Orc Grunt performs a basic attack!");
        animator.SetTrigger("MeleeAttackV1");
        player.GetComponent<IDamageable>().TakeDamage(7, DamageType.Physical);
    }

    private void MeleeAttackV2()
    {
        Debug.Log("Orc Grunt performs a power attack!");
        animator.SetTrigger("MeleeAttackV2");
        player.GetComponent<IDamageable>().TakeDamage(15, DamageType.Physical);
    }

    protected override void UpdateAnimationState()
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
}
