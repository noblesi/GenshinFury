using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcWarrior : OrcBase
{
    protected override void InitializeAttackPatterns()
    {
        attackPatterns.Add(() => BasicAttack());
    }

    private void BasicAttack()
    {
        Debug.Log("Slash");
    }
}
