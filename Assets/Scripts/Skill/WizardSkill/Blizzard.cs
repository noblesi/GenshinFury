using UnityEngine;

[CreateAssetMenu(fileName = "Blizzard", menuName = "Skill/Blizzard")]
public class Blizzard : SkillData
{
    private void OnEnable()
    {
        skillName = "Blizzard";
        types = new SkillType[] { SkillType.Ranged, SkillType.AreaOfEffect };
        damageType = DamageType.Magical;
        baseDamage = 0.9f;
        damagePerLevel = 0.2f;
        cooldowns = new float[] { 12.0f, 11.5f, 11.0f, 10.5f, 10.0f };
        effect = null;  // Assign appropriate ParticleSystem
        animationTrigger = "Skill2";
        key = KeyCode.W;
        level = 1;
        maxLevel = 5;
        evolvedSkill = null;  // No evolution
    }
}
