using UnityEngine;

[CreateAssetMenu(fileName = "PowerArrowShot", menuName = "Skill/PowerArrowShot")]
public class PowerArrowShot : SkillData
{
    private void OnEnable()
    {
        skillName = "Power Arrow Shot";
        types = new SkillType[] { SkillType.Ranged, SkillType.SingleTarget };
        damageType = DamageType.Physical;
        baseDamage = 1.5f;
        damagePerLevel = 0.3f;
        cooldowns = new float[] { 3.0f, 2.5f, 2.0f };
        manaCosts = new float[] { 20.0f, 22.0f, 24.0f };
        effect = null;  // Assign appropriate ParticleSystem
        animationTrigger = "Skill1";
        key = KeyCode.Q;
        level = 1;
        maxLevel = 3;
        evolvedSkill = null;  // No further evolution
    }
}
