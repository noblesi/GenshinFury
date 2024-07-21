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
        attackCooldown = 2f; // ���÷� �߰��� ���� ��ٿ� �ð�
        maxHealth = 80; // ������ �ִ� ü�� ����
        currentHealth = maxHealth; // ���� ü���� �ִ� ü������ ����
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
