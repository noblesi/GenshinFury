using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkeletonBase : Enemy
{
    protected Animator animator;
    private bool isAttacking;
    [SerializeField] protected List<ItemDrop> itemDrops;
    [SerializeField] private int experienceReward = 50;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        maxHealth = 50;
        currentHealth = maxHealth;
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

    public override void TakeDamage(int damage)
    {
        if (isDead) return;

        base.TakeDamage(damage);
        if (isAttacking)
        {
            animator.ResetTrigger("BasicAttack");
            isAttacking = false;
        }
        animator.SetTrigger("Hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected override void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("Enemy died.");

        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }

        if (agent != null)
        {
            agent.enabled = false;
        }

        animator.SetTrigger("Die");

        DropItems();

        // 전투 종료 이벤트 호출
        EventManager.Instance.BattleEnded();

        StartCoroutine(DestroyAfterDelay(2f));

        if (Player.Instance != null)
        {
            Player.Instance.GainExperience(experienceReward);
        }
    }

    private void DropItems()
    {
        LootManager lootManager = FindObjectOfType<LootManager>();
        if (lootManager == null)
        {
            Debug.LogError("LootManager not found in the scene.");
            return;
        }

        foreach (ItemDrop itemDrop in itemDrops)
        {
            lootManager.DropItem(transform.position, itemDrop);
        }
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
            Debug.Log($"{gameObject.name} collided with Player.");
            Player playerComponent = other.GetComponent<Player>();
            if (playerComponent != null && playerComponent.IsAttacking())
            {
                Debug.Log($"{gameObject.name} was hit by Player's attack.");
                int damage = playerComponent.CalculateDamage();
                Debug.Log($"Player deals {damage} damage to {gameObject.name}.");
                TakeDamage(damage);
            }
            else
            {
                Debug.Log($"{gameObject.name} was collided with Player, but Player is not attacking.");
            }
        }
    }
}
