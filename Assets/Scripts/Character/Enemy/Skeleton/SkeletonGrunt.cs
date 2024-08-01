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

        itemDrops = new List<ItemDrop>
        {
            new ItemDrop
            {
                item = Resources.Load<Item>("Items/HealthPotion"),
                minAmount = 1,
                maxAmount = 3,
                dropChance = 0.5f
            },
            new ItemDrop
            {
                item = Resources.Load<Item>("Items/Sword"),
                minAmount = 1,
                maxAmount = 1,
                dropChance = 0.2f
            },
            new ItemDrop
            {
                item = Resources.Load<Item>("Items/Helmet"),
                minAmount = 1,
                maxAmount = 1,
                dropChance = 0.15f
            },
            new ItemDrop
            {
                item = Resources.Load<Item>("Items/ChestArmor"),
                minAmount = 1,
                maxAmount = 1,
                dropChance = 0.1f
            }
        };
    }

    protected override void BasicAttack()
    {
        Debug.Log("Skeleton Grunt performs a basic attack!");
        animator.SetTrigger("BasicAttack");
        if (player != null)
        {
            player.GetComponent<IDamageable>().TakeDamage(attackDamage);
        }
    }
}