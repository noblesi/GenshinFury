using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(EnemyAI enemyAI) : base(enemyAI) { }

    public override void EnterState()
    {
        // ���� ���¿� ������ �� ������ �۾�
        Debug.Log("Entered Chase State");
    }

    public override void UpdateState()
    {
        // ���� ���¿����� ���� (��: �÷��̾� ����)
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
        // ���� ���¿��� ���� �� ������ �۾�
        Debug.Log("Exited Chase State");
        enemyAI.agent.ResetPath();
    }
}
