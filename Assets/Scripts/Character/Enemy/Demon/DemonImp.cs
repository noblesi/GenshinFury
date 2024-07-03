using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonImp : DemonBase
{
    protected override void InitializeAttackPatterns()
    {
        attackPatterns.Add(() => BasicAttack());
        attackPatterns.Add(() => Fireball());
    }

    private void BasicAttack()
    {
        Debug.Log("Demon Imp performs a basic attack!");
        animator.SetTrigger("BasicAttack");
        player.GetComponent<IDamageable>().TakeDamage(5, DamageType.Physical);
    }

    private void Fireball()
    {
        Debug.Log("Demon Imp casts a fireball!");
        animator.SetTrigger("Fireball");
        player.GetComponent<IDamageable>().TakeDamage(10, DamageType.Magical);
    }
}
