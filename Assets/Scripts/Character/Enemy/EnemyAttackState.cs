using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(EnemyAI enemyAI) : base(enemyAI) { }

    public override void EnterState()
    {
        // 공격 상태에 진입할 때 수행할 작업
        Debug.Log("Entered Attack State");
    }

    public override void UpdateState()
    {
        // 공격 상태에서의 동작 (예: 플레이어 공격)
        float distanceToTarget = Vector3.Distance(enemyAI.transform.position, enemyAI.target.position);

        if (distanceToTarget > enemyAI.attackRange)
        {
            enemyAI.SwitchState(enemyAI.chaseState);
        }
        else
        {
            if (Time.time > enemyAI.lastAttackTime + enemyAI.attackCooldown)
            {
                Attack();
            }
        }
    }

    public override void ExitState()
    {
        // 공격 상태에서 나갈 때 수행할 작업
        Debug.Log("Exited Attack State");
    }

    private void Attack()
    {
        // 공격 로직 구현
        Debug.Log("Enemy attacks the target");
        enemyAI.lastAttackTime = Time.time;
    }
}
