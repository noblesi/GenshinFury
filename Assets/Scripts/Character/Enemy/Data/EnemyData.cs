using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public EnemyType enemyType;
    public float detectionRange;
    public float attackRange;
    public float attackCooldown;
    public AnimationClip[] idleAnimations;
    public AnimationClip[] attackAnimations;
}
