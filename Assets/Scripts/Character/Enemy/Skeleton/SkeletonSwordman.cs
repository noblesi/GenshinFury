using System.Collections;
using UnityEngine;

public class SkeletonSwordman : SkeletonBase
{
    protected override void Start()
    {
        base.Start();
        detectionRange = 15f;
        attackRange = 3f;
        attackDamage = 10;
        attackCooldown = 2f; // 예시로 추가한 공격 쿨다운 시간
        maxHealth = 80; // 몬스터의 최대 체력 설정
        currentHealth = maxHealth; // 현재 체력을 최대 체력으로 설정
    }

    protected override void BasicAttack()
    {
        Debug.Log("Skeleton Swordman performs a basic attack!");
        animator.SetTrigger("BasicAttack");
        if (player != null)
        {
            player.GetComponent<IDamageable>().TakeDamage(attackDamage, DamageType.Physical);
        }
    }
}
