using System.Collections;
using UnityEngine;

public class OrcWarrior : OrcBase
{
    private bool isAttacking = false;

    protected override void InitializeAttackPatterns()
    {
        attackPatterns.Add(() => StartCoroutine(PerformAttack(Slash)));
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

    private void Slash()
    {
        Debug.Log("Slash");
        animator.SetTrigger("Slash");
        player.GetComponent<IDamageable>().TakeDamage(7, DamageType.Physical);
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
