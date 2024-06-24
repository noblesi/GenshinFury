using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(EnemyAI enemyAI) : base(enemyAI) { }

    public override void EnterState()
    {
        // ���� ���¿� ������ �� ������ �۾�
        Debug.Log("Entered Attack State");
    }

    public override void UpdateState()
    {
        // ���� ���¿����� ���� (��: �÷��̾� ����)
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
        // ���� ���¿��� ���� �� ������ �۾�
        Debug.Log("Exited Attack State");
    }

    private void Attack()
    {
        // ���� ���� ����
        Debug.Log("Enemy attacks the target");
        enemyAI.lastAttackTime = Time.time;
    }
}
