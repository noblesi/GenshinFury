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
        attackCooldown = 1.5f; // ���÷� �߰��� ���� ��ٿ� �ð�
        maxHealth = 50; // ������ �ִ� ü�� ����
        currentHealth = maxHealth; // ���� ü���� �ִ� ü������ ����

        // ��� ����Ʈ ����
        itemDrops = new List<ItemDrop>
        {
            new ItemDrop
            {
                item = Resources.Load<Item>("Items/HealthPotion"), // HealthPotion ������ ����
                minAmount = 1,
                maxAmount = 3,
                dropChance = 0.5f // 50% Ȯ���� ���
            },
            new ItemDrop
            {
                item = Resources.Load<Item>("Items/Sword"), // Sword ������ ����
                minAmount = 1,
                maxAmount = 1,
                dropChance = 0.2f // 20% Ȯ���� ���
            },
            new ItemDrop
            {
                item = Resources.Load<Item>("Items/Helmet"), // Helmet ������ ����
                minAmount = 1,
                maxAmount = 1,
                dropChance = 0.15f // 15% Ȯ���� ���
            },
            new ItemDrop
            {
                item = Resources.Load<Item>("Items/ChestArmor"), // ChestArmor ������ ����
                minAmount = 1,
                maxAmount = 1,
                dropChance = 0.1f // 10% Ȯ���� ���
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
