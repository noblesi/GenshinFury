using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : Enemy
{
    protected override void PerformAttack()
    {
        Debug.Log("Demon Attack");
    }
}
