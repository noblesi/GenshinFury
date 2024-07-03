using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonFatty : DemonBase
{
    protected override void InitializeAttackPatterns()
    {
        attackPatterns.Add(() => BodySlam());
        attackPatterns.Add(() => BellyFlop());
    }

    private void BodySlam()
    {
        Debug.Log("Demon Fatty performs a body slam!");
        animator.SetTrigger("BodySlam");
        player.GetComponent<IDamageable>().TakeDamage(15, DamageType.Physical);
    }

    private void BellyFlop()
    {
        Debug.Log("Demon Fatty performs a belly flop!");
        animator.SetTrigger("BellyFlop");
        player.GetComponent<IDamageable>().TakeDamage(18, DamageType.Physical);
    }
}
