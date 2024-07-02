using UnityEngine;

public class OrcGrunt : OrcBase
{
    protected override void InitializeAttackPatterns()
    {
        attackPatterns.Add(() => BasicAttack());
        attackPatterns.Add(() => PowerAttack());
    }

    private void BasicAttack()
    {
        Debug.Log("Orc Grunt performs a basic attack!");
        player.GetComponent<IDamageable>().TakeDamage(7, DamageType.Physical);
    }

    private void PowerAttack()
    {
        Debug.Log("Orc Grunt performs a power attack!");
        player.GetComponent<IDamageable>().TakeDamage(15, DamageType.Physical);
    }
}
