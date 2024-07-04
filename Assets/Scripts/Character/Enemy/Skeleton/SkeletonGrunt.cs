using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillData;

public class SkeletonGrunt : SkeletonBase
{
    protected override void InitializeAttackPatterns()
    {
        attackPatterns.Add(() => BasicAttack());
        attackPatterns.Add(() => HeavySwing());
    }

    private void BasicAttack()
    {
        Debug.Log("Skeleton Grunt performs a basic attack!");
        animator.SetTrigger("BasicAttack");
        player.GetComponent<IDamageable>().TakeDamage(7, DamageType.Physical);
    }

    private void HeavySwing()
    {
        Debug.Log("Skeleton Grunt performs a heavy swing!");
        animator.SetTrigger("HeavySwing");
        player.GetComponent<IDamageable>().TakeDamage(12, DamageType.Physical);
    }
}
