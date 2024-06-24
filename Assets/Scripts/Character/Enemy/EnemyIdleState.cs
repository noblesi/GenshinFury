using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(EnemyAI enemyAI) : base(enemyAI) { }

    public override void EnterState()
    {
        // ��� ���¿� ������ �� ������ �۾�
        Debug.Log("Entered Idle State");
    }

    public override void UpdateState()
    {
        // ��� ���¿����� ���� (��: �÷��̾� Ž��)
        float distanceToTarget = Vector3.Distance(enemyAI.transform.position, enemyAI.target.position);
        if (distanceToTarget <= enemyAI.detectionRange)
        {
            enemyAI.SwitchState(enemyAI.chaseState);
        }
    }

    public override void ExitState()
    {
        // ��� ���¿��� ���� �� ������ �۾�
        Debug.Log("Exited Idle State");
    }
}
