using System.Collections;
using UnityEngine;

public abstract class SkeletonBase : Enemy
{
    protected Animator animator;

    private bool isAttacking;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        EnsureComponents();

        // ������ ü�� ����
        maxHealth = 50; // �⺻ ��
        currentHealth = maxHealth;
    }

    private void EnsureComponents()
    {
        EnsureComponent<Collider>(gameObject);
        EnsureRigidbody(gameObject);
    }

    protected override void Update()
    {
        base.Update();
        if (animator != null)
        {
            UpdateAnimationState();
            if (player != null)
            {
                CheckAttackRange();
            }
        }
    }

    protected virtual void UpdateAnimationState()
    {
        switch (currentState)
        {
            case MonsterState.Patrol:
                animator.SetBool("isChasing", false);
                animator.SetBool("isAttacking", false);
                break;
            case MonsterState.Chase:
                animator.SetBool("isChasing", true);
                animator.SetBool("isAttacking", false);
                break;
            case MonsterState.Attack:
                animator.SetBool("isChasing", false);
                animator.SetBool("isAttacking", true);
                break;
        }
    }

    protected override void PerformAttack()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            isAttacking = true;
            BasicAttack();
            lastAttackTime = Time.time;
        }
    }

    protected abstract void BasicAttack();

    public override void TakeDamage(int damage, DamageType damageType)
    {
        base.TakeDamage(damage, damageType);
        if (isAttacking)
        {
            animator.ResetTrigger("BasicAttack"); // ���� �ִϸ��̼� ���
            isAttacking = false;
        }
        animator.SetTrigger("Hit");

        // ����� �޽����� ���� ü�� ���
        Debug.Log($"{gameObject.name} took {damage} damage. Current health: {currentHealth}");

        // ü���� 0 ���Ϸ� �������� Die �޼��� ȣ��
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected override void Die()
    {
        Debug.Log("Enemy died.");
        animator.SetTrigger("Die"); // �״� �ִϸ��̼� Ʈ����
        StartCoroutine(DestroyAfterDelay(2f)); // 2�� �� ����
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    protected virtual void CheckAttackRange()
    {
        if (player == null) return;

        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            currentState = MonsterState.Attack;
        }
        else if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            currentState = MonsterState.Chase;
        }
        else
        {
            currentState = MonsterState.Patrol;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TakeDamage(other.GetComponent<Player>().CalculateDamage(), DamageType.Physical);
        }
    }
}
