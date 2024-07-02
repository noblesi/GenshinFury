using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int damage, DamageType damageType);
    void ApplyDebuff(SkillData debuffSkill);
}
