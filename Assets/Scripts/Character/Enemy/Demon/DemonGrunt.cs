using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonGrunt : DemonBase
{
    protected override void InitializeAttackPatterns()
    {
        attackPatterns.Add(() => BasicAttack());
        attackPatterns.Add(() => ChargeAttack());
    }

    private void BasicAttack()
    {
        Debug.Log("Demon Grunt performs a basic attack!");
        animator.SetTrigger("BasicAttack");
        player.GetComponent<IDamageable>().TakeDamage(8, DamageType.Physical);
    }

    private void ChargeAttack()
    {
        Debug.Log("Demon Grunt performs a charge attack!");
        animator.SetTrigger("ChargeAttack");
        player.GetComponent<IDamageable>().TakeDamage(12, DamageType.Physical);
    }
}
