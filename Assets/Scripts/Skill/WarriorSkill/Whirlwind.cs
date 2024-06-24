using UnityEngine;

[CreateAssetMenu(fileName = "Whirlwind", menuName = "Skill/Whirlwind")]
public class Whirlwind : SkillData
{
    private void OnEnable()
    {
        skillName = "Whirlwind";
        types = new SkillType[] { SkillType.Melee, SkillType.AreaOfEffect };
        damageType = DamageType.Physical;
        baseDamage = 0.8f;
        damagePerLevel = 0.1f;
        cooldowns = new float[] { 8.0f, 7.5f, 7.0f, 6.5f, 6.0f };
        manaCosts = new float[] { 20.0f, 22.0f, 24.0f, 26.0f, 28.0f };
        effect = null;  // Assign appropriate ParticleSystem
        animationTrigger = "Skill2";
        key = KeyCode.W;
        level = 1;
        maxLevel = 5;
        evolvedSkill = null;
    }
}
