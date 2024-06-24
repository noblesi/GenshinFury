using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent agent;

    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    public float lastAttackTime;

    public EnemyState currentState;
    public EnemyIdleState idleState;
    public EnemyChaseState chaseState;
    public EnemyAttackState attackState;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        idleState = new EnemyIdleState(this);
        chaseState = new EnemyChaseState(this);
        attackState = new EnemyAttackState(this);

        currentState = idleState;
        currentState.EnterState();
    }

    void Update()
    {
        currentState.UpdateState();
    }

    public void SwitchState(EnemyState newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }
}
