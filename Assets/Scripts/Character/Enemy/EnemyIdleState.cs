using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(EnemyAI enemyAI) : base(enemyAI) { }

    public override void EnterState()
    {
        // 대기 상태에 진입할 때 수행할 작업
        Debug.Log("Entered Idle State");
    }

    public override void UpdateState()
    {
        // 대기 상태에서의 동작 (예: 플레이어 탐지)
        float distanceToTarget = Vector3.Distance(enemyAI.transform.position, enemyAI.target.position);
        if (distanceToTarget <= enemyAI.detectionRange)
        {
            enemyAI.SwitchState(enemyAI.chaseState);
        }
    }

    public override void ExitState()
    {
        // 대기 상태에서 나갈 때 수행할 작업
        Debug.Log("Exited Idle State");
    }
}
