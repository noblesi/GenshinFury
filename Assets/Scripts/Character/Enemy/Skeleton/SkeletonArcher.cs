using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArcher : SkeletonBase
{
    protected override void InitializeAttackPatterns()
    {
        attackPatterns.Add(() => BasicAttack());
        attackPatterns.Add(() => PowerShot());
    }

    private void BasicAttack()
    {
        Debug.Log("Skeleton Archer performs a basic attack!");
        animator.SetTrigger("BasicAttack");
        player.GetComponent<IDamageable>().TakeDamage(5, DamageType.Physical);
    }

    private void PowerShot()
    {
        Debug.Log("Skeleton Archer performs a power shot!");
        animator.SetTrigger("PowerShot");
        player.GetComponent<IDamageable>().TakeDamage(10, DamageType.Physical);
    }
}
