using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillData;

public class SkeletonMage : SkeletonBase
{
    protected override void InitializeAttackPatterns()
    {
        attackPatterns.Add(() => BasicAttack());
        attackPatterns.Add(() => Fireball());
        attackPatterns.Add(() => IceBlast());
    }

    private void BasicAttack()
    {
        Debug.Log("Skeleton Mage performs a basic attack!");
        animator.SetTrigger("BasicAttack");
        player.GetComponent<IDamageable>().TakeDamage(6, DamageType.Magical);
    }

    private void Fireball()
    {
        Debug.Log("Skeleton Mage casts a fireball!");
        animator.SetTrigger("Fireball");
        player.GetComponent<IDamageable>().TakeDamage(10, DamageType.Magical);
    }

    private void IceBlast()
    {
        Debug.Log("Skeleton Mage casts an ice blast!");
        animator.SetTrigger("IceBlast");
        player.GetComponent<IDamageable>().TakeDamage(12, DamageType.Magical);
    }
}
