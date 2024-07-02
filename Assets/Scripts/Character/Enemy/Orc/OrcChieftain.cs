using UnityEngine;

public class OrcChieftain : OrcBase
{
    protected override void InitializeAttackPatterns()
    {
        attackPatterns.Add(() => BasicAttack());
        attackPatterns.Add(() => PowerAttack());
        attackPatterns.Add(() => ChargeAttack());
        attackPatterns.Add(() => SlamAttack());
    }

    private void BasicAttack()
    {
        Debug.Log("Orc Chieftain performs a basic attack!");
        player.GetComponent<IDamageable>().TakeDamage(5, DamageType.Physical);
    }

    private void PowerAttack()
    {
        Debug.Log("Orc Chieftain performs a power attack!");
        player.GetComponent<IDamageable>().TakeDamage(10, DamageType.Physical);
    }

    private void ChargeAttack()
    {
        Debug.Log("Orc Chieftain performs a charge attack!");
        player.GetComponent<IDamageable>().TakeDamage(12, DamageType.Physical);
    }

    private void SlamAttack()
    {
        Debug.Log("Orc Chieftain performs a slam attack!");
        player.GetComponent<IDamageable>().TakeDamage(20, DamageType.Physical);
    }
}
