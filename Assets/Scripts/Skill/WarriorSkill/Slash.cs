using UnityEngine;

[CreateAssetMenu(fileName = "Slash", menuName = "Skill/Slash")]
public class Slash : SkillData
{
    private void OnEnable()
    {
        skillName = "Slash";
        types = new SkillType[] { SkillType.Melee, SkillType.SingleTarget };
        damageType = DamageType.Physical;
        baseDamage = 1.0f;
        damagePerLevel = 0.2f;
        cooldowns = new float[] { 5.0f, 4.5f, 4.0f, 3.5f, 3.0f };
        effect = null;  // Assign appropriate ParticleSystem
        animationTrigger = "Skill1";
        key = KeyCode.Q;
        level = 1;
        maxLevel = 5;
        evolvedSkill = Resources.Load<SkillData>("SkillData/PowerSlash"); // Load the evolved skill
    }
}
