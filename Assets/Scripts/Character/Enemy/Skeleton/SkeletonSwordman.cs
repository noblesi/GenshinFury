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
        attackCooldown = 2f; // ���÷� �߰��� ���� ��ٿ� �ð�
        maxHealth = 80; // ������ �ִ� ü�� ����
        currentHealth = maxHealth; // ���� ü���� �ִ� ü������ ����

        // ��� ����Ʈ ����
        itemDrops = new List<ItemDrop>
        {
            new ItemDrop
            {
                item = Resources.Load<Item>("Items/ManaPotion"), // ManaPotion ������ ����
                minAmount = 1,
                maxAmount = 2,
                dropChance = 0.5f // 50% Ȯ���� ���
            },
            new ItemDrop
            {
                item = Resources.Load<Item>("Items/Sword"), // Sword ������ ����
                minAmount = 1,
                maxAmount = 1,
                dropChance = 0.3f // 30% Ȯ���� ���
            },
            new ItemDrop
            {
                item = Resources.Load<Item>("Items/LegArmor"), // LegArmor ������ ����
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
