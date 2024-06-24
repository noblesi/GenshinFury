using UnityEngine;

[CreateAssetMenu(fileName = "Curse", menuName = "Skill/Curse")]
public class Curse : SkillData
{
    private void OnEnable()
    {
        skillName = "Curse";
        types = new SkillType[] { SkillType.Debuff };
        damageType = DamageType.Magical;
        baseDamage = 0.0f; // Debuff doesn't deal direct damage
        damagePerLevel = 0.0f;
        cooldowns = new float[] { 20.0f, 18.0f, 16.0f, 14.0f, 12.0f };
        manaCosts = new float[] { 20.0f, 22.0f, 24.0f, 26.0f, 28.0f };
        effect = null;  // Assign appropriate ParticleSystem
        animationTrigger = "Skill3";
        key = KeyCode.E;
        level = 1;
        maxLevel = 5;
        evolvedSkill = null;
    }
}
