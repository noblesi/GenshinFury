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
        EnsureComponents();
        maxHealth = 50;
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

    public override void TakeDamage(int damage)
    {
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
        Debug.Log("Enemy died.");
        animator.SetTrigger("Die");
        StartCoroutine(DestroyAfterDelay(2f));
        DropItems();
        if (Player.Instance != null)
        {
            Player.Instance.GainExperience(experienceReward);
        }
    }

    private void DropItems()
    {
        foreach (ItemDrop itemDrop in itemDrops)
        {
            if (Random.value <= itemDrop.dropChance)
            {
                int amount = Random.Range(itemDrop.minAmount, itemDrop.maxAmount + 1);
                for (int i = 0; i < amount; i++)
                {
                    Vector3 dropPosition = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
                    GameObject drop = Instantiate(itemDrop.item.itemPrefab, dropPosition, Quaternion.identity);

                    Equipment equipment = itemDrop.item as Equipment;
                    if (equipment != null)
                    {
                        equipment.AssignRandomStats();
                    }

                    drop.GetComponent<Loot>().item = itemDrop.item;
                    drop.GetComponent<Loot>().amount = 1;
                }
            }
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
            TakeDamage(other.GetComponent<Player>().CalculateDamage());
        }
    }
}
