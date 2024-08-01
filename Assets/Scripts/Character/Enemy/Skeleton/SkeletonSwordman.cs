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

        itemDrops = new List<ItemDrop>
        {
            new ItemDrop
            {
                item = Resources.Load<Item>("Items/ManaPotion"),
                minAmount = 1,
                maxAmount = 2,
                dropChance = 0.5f
            },
            new ItemDrop
            {
                item = Resources.Load<Item>("Items/Sword"),
                minAmount = 1,
                maxAmount = 1,
                dropChance = 0.3f
            },
            new ItemDrop
            {
                item = Resources.Load<Item>("Items/LegArmor"),
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
            }
        };
    }

    protected override void BasicAttack()
    {
        Debug.Log("Skeleton Swordman performs a basic attack!");
        animator.SetTrigger("BasicAttack");
        if (player != null)
        {
            player.GetComponent<IDamageable>().TakeDamage(attackDamage);
        }
    }
}