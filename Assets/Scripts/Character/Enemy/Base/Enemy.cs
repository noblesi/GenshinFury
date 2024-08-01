using UnityEngine;

public abstract class Enemy : BaseCharacter
{
    public float detectionRange;
    public float attackRange;
    public float attackCooldown;
    public int attackDamage;
    protected float lastAttackTime;
    protected Transform player;
    protected MonsterState currentState;

    protected override void Start()
    {
        base.Start();
        GameManager.Instance.OnPlayerCreated += OnPlayerCreated;
        currentState = MonsterState.Patrol;
    }

    protected virtual void Update()
    {
        if (player == null) return;

        switch (currentState)
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

    protected virtual void OnPlayerCreated(Player player)
    {
        this.player = player.transform;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            Player player = other.GetComponentInParent<Player>();
            if (player != null && player.IsAttacking())
            {
                TakeDamage(player.CalculateDamage());
                Debug.Log($"{gameObject.name} was hit by {player.name}");
            }
        }
    }

    protected override void Die()
    {
        Debug.Log("Enemy died.");
        Destroy(gameObject);
    }

    protected abstract void PerformAttack();

    protected virtual void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerCreated -= OnPlayerCreated;
        }
    }
}