using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSwordman : SkeletonBase
{
    protected override void InitializeAttackPatterns()
    {
        attackPatterns.Add(() => BasicAttack());
        attackPatterns.Add(() => SwordSlash());
        attackPatterns.Add(() => ShieldBash());
    }

    private void BasicAttack()
    {
        Debug.Log("Skeleton Swordman performs a basic attack!");
        animator.SetTrigger("BasicAttack");
        player.GetComponent<IDamageable>().TakeDamage(7, DamageType.Physical);
    }

    private void SwordSlash()
    {
        Debug.Log("Skeleton Swordman performs a sword slash!");
        animator.SetTrigger("SwordSlash");
        player.GetComponent<IDamageable>().TakeDamage(12, DamageType.Physical);
    }

    private void ShieldBash()
    {
        Debug.Log("Skeleton Swordman performs a shield bash!");
        animator.SetTrigger("ShieldBash");
        player.GetComponent<IDamageable>().TakeDamage(10, DamageType.Physical);
    }
}
