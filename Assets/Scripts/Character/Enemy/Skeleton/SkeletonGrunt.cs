using System.Collections;
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
    }

    protected override void BasicAttack()
    {
        Debug.Log("Skeleton Grunt performs a basic attack!");
        animator.SetTrigger("BasicAttack");
        if (player != null)
        {
            player.GetComponent<IDamageable>().TakeDamage(attackDamage, DamageType.Physical);
        }
    }
}
