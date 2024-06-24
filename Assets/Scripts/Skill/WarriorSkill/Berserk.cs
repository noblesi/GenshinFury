using UnityEngine;

[CreateAssetMenu(fileName = "Berserk", menuName = "Skill/Berserk")]
public class Berserk : SkillData
{
    private void OnEnable()
    {
        skillName = "Berserk";
        types = new SkillType[] { SkillType.Melee, SkillType.Buff };
        damageType = DamageType.Physical;
        baseDamage = 2.0f;
        damagePerLevel = 0.5f;
        cooldowns = new float[] { 60.0f, 55.0f, 50.0f };
        manaCosts = new float[] { 50.0f, 55.0f, 60.0f };
        effect = null;  // Assign appropriate ParticleSystem
        animationTrigger = "Skill4";
        key = KeyCode.R;
        level = 1;
        maxLevel = 3;
        evolvedSkill = null;
    }
}
