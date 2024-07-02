using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : BaseCharacter
{
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    protected float lastAttackTime;
    protected Transform player;
    protected MonsterState currentState;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = MonsterState.Patrol;
    }

    protected virtual void Update()
    {
        switch(currentState)
        {
            case MonsterState.Patrol:
                Patrol();
                break;
            case MonsterState.Chase:
                Chase();
                break;
            case MonsterState.Attack:
                Attack();
                break;
        }
    }

    protected virtual void Patrol()
    {
        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            currentState = MonsterState.Chase;
        }
    }

    protected virtual void Chase()
    {
        agent.SetDestination(player.position);
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            currentState = MonsterState.Attack;
        }
        else if (Vector3.Distance(transform.position, player.position) > detectionRange)
        {
            currentState = MonsterState.Patrol;
        }
    }

    protected virtual void Attack()
    {
        agent.SetDestination(transform.position);
        if (Time.time - lastAttackTime > attackCooldown)
        {
            PerformAttack();
            lastAttackTime = Time.time;
        }

        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            currentState = MonsterState.Chase;
        }
    }

    protected abstract void PerformAttack();

    protected override void Die()
    {
        Debug.Log("Enemy died.");
        Destroy(gameObject);
    }
}
