using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(EnemyAI enemyAI) : base(enemyAI) { }

    public override void EnterState()
    {
        // 추적 상태에 진입할 때 수행할 작업
        Debug.Log("Entered Chase State");
    }

    public override void UpdateState()
    {
        // 추적 상태에서의 동작 (예: 플레이어 추적)
        float distanceToTarget = Vector3.Distance(enemyAI.transform.position, enemyAI.target.position);
        enemyAI.agent.SetDestination(enemyAI.target.position);

        if (distanceToTarget <= enemyAI.attackRange)
        {
            enemyAI.SwitchState(enemyAI.attackState);
        }
        else if (distanceToTarget > enemyAI.detectionRange)
        {
            enemyAI.SwitchState(enemyAI.idleState);
        }
    }

    public override void ExitState()
    {
        // 추적 상태에서 나갈 때 수행할 작업
        Debug.Log("Exited Chase State");
        enemyAI.agent.ResetPath();
    }
}
