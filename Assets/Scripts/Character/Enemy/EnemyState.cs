using UnityEngine;

public abstract class EnemyState
{
    protected EnemyAI enemyAI;

    public EnemyState(EnemyAI enemyAI)
    {
        this.enemyAI = enemyAI;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}
