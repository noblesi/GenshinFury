using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkeletonBase : Enemy
{
    [SerializeField] private int experienceReward = 50;
    private bool isAttacking;

    protected override void Start()
    {
        base.Start();
        maxHealth = 50;
        currentHealth = maxHealth;
    }

    protected override void Update()
    {
        base.Update();
        UpdateAnimationState();
        if (player != null)
        {
            CheckAttackRange();
        }
    }

    protected virtual void UpdateAnimationState()
    {
        if (animator == null) return;

        animator.SetBool("isChasing", currentState == MonsterState.Chase);
        animator.SetBool("isAttacking", currentState == MonsterState.Attack);
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
            ResetAnimationTrigger(animator, "BasicAttack");
            isAttacking = false;
        }
        PlayAnimation(animator, "Hit");
    }

    protected override void Die()
    {
        base.Die();
        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }

        if (agent != null)
        {
            agent.enabled = false;
        }

        EventManager.Instance.BattleEnded();

        StartCoroutine(DestroyAfterDelay(2f));

        if (Player.Instance != null)
        {
            Player.Instance.GainExperience(experienceReward);
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
            Player playerComponent = other.GetComponent<Player>();
            if (playerComponent != null && playerComponent.IsAttacking())
            {
                int damage = playerComponent.CalculateDamage();
                TakeDamage(damage);
            }
        }
    }
}