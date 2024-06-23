using UnityEngine;

[CreateAssetMenu(fileName = "Inferno", menuName = "Skill/Inferno")]
public class Inferno : SkillData
{
    private void OnEnable()
    {
        skillName = "Inferno";
        types = new SkillType[] { SkillType.Ranged, SkillType.AreaOfEffect };
        damageType = DamageType.Magical;
        baseDamage = 1.5f;
        damagePerLevel = 0.4f;
        cooldowns = new float[] { 5.0f, 4.5f, 4.0f };
        effect = null;  // Assign appropriate ParticleSystem
        animationTrigger = "Skill1";
        key = KeyCode.Q;
        level = 1;
        maxLevel = 3;
        evolvedSkill = null;  // No further evolution
    }
}
