using UnityEngine;

public class OrcChieftain : OrcBase
{
    private int idleIndex = 0;

    protected override void Start()
    {
        base.Start();
        InvokeRepeating(nameof(SwitchIdleAnimation), 2f, 5f);
    }
    protected override void InitializeAttackPatterns()
    {
        attackPatterns.Add(() => MeleeAttack());
        attackPatterns.Add(() => ComboAttackV1());
        attackPatterns.Add(() => ComboAttackV2());
        attackPatterns.Add(() => ComboAttackV3());
    }

    private void SwitchIdleAnimation()
    {
        if(currentState == MonsterState.Patrol)
        {
            idleIndex = 1 - idleIndex;
            animator.SetFloat("IdleIndex", idleIndex);
        }
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
                animator.SetFloat("IdleIndex", 0f);
                animator.SetBool("isChasing", true);
                animator.SetBool("isAttacking", false);
                break;
            case MonsterState.Attack:
                animator.SetFloat("IdleIndex", 0f);
                animator.SetBool("isChasing", false);
                animator.SetBool("isAttacking", true);
                break;
        }
    }

    private void MeleeAttack()
    {
        Debug.Log("Orc Chieftain performs a MeleeAttack!");
        animator.SetTrigger("MeleeAttack");
        player.GetComponent<IDamageable>().TakeDamage(5, DamageType.Physical);
    }

    private void ComboAttackV1()
    {
        Debug.Log("Orc Chieftain performs a ComboAttack Version1");
        animator.SetTrigger("ComboAttackV1");
        player.GetComponent<IDamageable>().TakeDamage(10, DamageType.Physical);
    }

    private void ComboAttackV2()
    {
        Debug.Log("Orc Chieftain performs a ComboAttack Version2!");
        animator.SetTrigger("ComboAttackV2");
        player.GetComponent<IDamageable>().TakeDamage(12, DamageType.Physical);
    }

    private void ComboAttackV3()
    {
        Debug.Log("Orc Chieftain performs a ComboAttack Version3!");
        animator.SetTrigger("ComboAttackV3");
        player.GetComponent<IDamageable>().TakeDamage(20, DamageType.Physical);
    }
}
