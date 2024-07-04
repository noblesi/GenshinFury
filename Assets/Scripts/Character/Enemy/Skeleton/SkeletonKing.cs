using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillData;

public class SkeletonKing : SkeletonBase
{
    protected override void InitializeAttackPatterns()
    {
        attackPatterns.Add(() => BasicAttack());
        attackPatterns.Add(() => RoyalStrike());
        attackPatterns.Add(() => SummonMinions());
    }

    private void BasicAttack()
    {
        Debug.Log("Skeleton King performs a basic attack!");
        animator.SetTrigger("BasicAttack");
        player.GetComponent<IDamageable>().TakeDamage(8, DamageType.Physical);
    }

    private void RoyalStrike()
    {
        Debug.Log("Skeleton King performs a royal strike!");
        animator.SetTrigger("RoyalStrike");
        player.GetComponent<IDamageable>().TakeDamage(15, DamageType.Physical);
    }

    private void SummonMinions()
    {
        Debug.Log("Skeleton King summons minions!");
        animator.SetTrigger("SummonMinions");
        // Logic to summon minions
    }
}
