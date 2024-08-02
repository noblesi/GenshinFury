using System.Collections.Generic;
using UnityEngine;

public class SkeletonGrunt : SkeletonBase
{
    protected override void Start()
    {
        base.Start();
        detectionRange = 10f;
        attackRange = 2f;
        attackDamage = 5;
        attackCooldown = 1.5f;
        maxHealth = 50;
        currentHealth = maxHealth;
    }

    protected override void BasicAttack()
    {
        PlayAnimation(animator, "BasicAttack");
        if (player != null)
        {
            player.GetComponent<IDamageable>().TakeDamage(attackDamage);
        }
    }
}