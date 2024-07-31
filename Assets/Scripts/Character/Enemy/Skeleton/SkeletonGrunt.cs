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
        attackCooldown = 1.5f; // 예시로 추가한 공격 쿨다운 시간
        maxHealth = 50; // 몬스터의 최대 체력 설정
        currentHealth = maxHealth; // 현재 체력을 최대 체력으로 설정

        // 드롭 리스트 설정
        itemDrops = new List<ItemDrop>
        {
            new ItemDrop
            {
                item = Resources.Load<Item>("Items/HealthPotion"), // HealthPotion 아이템 참조
                minAmount = 1,
                maxAmount = 3,
                dropChance = 0.5f // 50% 확률로 드롭
            },
            new ItemDrop
            {
                item = Resources.Load<Item>("Items/Sword"), // Sword 아이템 참조
                minAmount = 1,
                maxAmount = 1,
                dropChance = 0.2f // 20% 확률로 드롭
            },
            new ItemDrop
            {
                item = Resources.Load<Item>("Items/Helmet"), // Helmet 아이템 참조
                minAmount = 1,
                maxAmount = 1,
                dropChance = 0.15f // 15% 확률로 드롭
            },
            new ItemDrop
            {
                item = Resources.Load<Item>("Items/ChestArmor"), // ChestArmor 아이템 참조
                minAmount = 1,
                maxAmount = 1,
                dropChance = 0.1f // 10% 확률로 드롭
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
