using UnityEngine;

[CreateAssetMenu(fileName = "PowerSlash", menuName = "Skill/PowerSlash")]
public class PowerSlash : SkillData
{
    private void OnEnable()
    {
        skillName = "Power Slash";
        types = new SkillType[] { SkillType.Melee, SkillType.SingleTarget };
        damageType = DamageType.Physical;
        baseDamage = 1.5f;
        damagePerLevel = 0.3f;
        cooldowns = new float[] { 4.0f, 3.5f, 3.0f };
        manaCosts = new float[] { 20.0f, 22.0f, 24.0f };
        effect = null;  // Assign appropriate ParticleSystem
        animationTrigger = "Skill1";
        key = KeyCode.Q;
        level = 1;
        maxLevel = 3;
        evolvedSkill = null;  // No further evolution
    }
}
