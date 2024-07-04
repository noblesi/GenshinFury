using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillData;

public class DemonBoss : DemonBase
{
    protected override void InitializeAttackPatterns()
    {
        attackPatterns.Add(() => BasicAttack());
        attackPatterns.Add(() => HeavyStrike());
        attackPatterns.Add(() => SummonMinions());
        attackPatterns.Add(() => FireBreath());
    }

    private void BasicAttack()
    {
        Debug.Log("Demon Boss performs a basic attack!");
        animator.SetTrigger("BasicAttack");
        player.GetComponent<IDamageable>().TakeDamage(10, DamageType.Physical);
    }

    private void HeavyStrike()
    {
        Debug.Log("Demon Boss performs a heavy strike!");
        animator.SetTrigger("HeavyStrike");
        player.GetComponent<IDamageable>().TakeDamage(15, DamageType.Physical);
    }

    private void SummonMinions()
    {
        Debug.Log("Demon Boss summons minions!");
        animator.SetTrigger("SummonMinions");
        // Logic to summon minions
    }

    private void FireBreath()
    {
        Debug.Log("Demon Boss performs a fire breath!");
        animator.SetTrigger("FireBreath");
        player.GetComponent<IDamageable>().TakeDamage(20, DamageType.Magical);
    }
}
