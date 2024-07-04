using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillData;

public class DemonBomb : DemonBase
{
    protected override void InitializeAttackPatterns()
    {
        attackPatterns.Add(() => Explode());
    }

    private void Explode()
    {
        Debug.Log("Demon Bomb explodes!");
        animator.SetTrigger("Explode");
        player.GetComponent<IDamageable>().TakeDamage(20, DamageType.Physical);
        Destroy(gameObject); // Æø¹ß ÈÄ ÀÚ½ÅÀ» ÆÄ±«
    }
}
