using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillData;

public interface IDamageable
{
    void TakeDamage(int damage, DamageType damageType);
    void ApplyDebuff(SkillData debuffSkill);
}
