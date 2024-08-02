using System.Collections.Generic;
using UnityEngine;

public class SkeletonSwordman : SkeletonBase
{
    protected override void Start()
    {
        base.Start();
        detectionRange = 15f;
        attackRange = 3f;
        attackDamage = 10;
        attackCooldown = 2f;
        maxHealth = 80;
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