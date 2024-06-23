using UnityEngine;

[CreateAssetMenu(fileName = "PiercingArrow", menuName = "Skill/PiercingArrow")]
public class PiercingArrow : SkillData
{
    private void OnEnable()
    {
        skillName = "Piercing Arrow";
        types = new SkillType[] { SkillType.Ranged, SkillType.SingleTarget };
        damageType = DamageType.Physical;
        baseDamage = 2.0f;
        damagePerLevel = 0.5f;
        cooldowns = new float[] { 60.0f, 55.0f, 50.0f };
        effect = null;  // Assign appropriate ParticleSystem
        animationTrigger = "Skill4";
        key = KeyCode.R;
        level = 1;
        maxLevel = 3;
        evolvedSkill = null;
    }
}
