using UnityEngine;

[CreateAssetMenu(fileName = "RainOfArrows", menuName = "Skill/RainOfArrows")]
public class RainOfArrows : SkillData
{
    private void OnEnable()
    {
        skillName = "Rain of Arrows";
        types = new SkillType[] { SkillType.Ranged, SkillType.AreaOfEffect };
        damageType = DamageType.Physical;
        baseDamage = 0.8f;
        damagePerLevel = 0.1f;
        cooldowns = new float[] { 10.0f, 9.5f, 9.0f, 8.5f, 8.0f };
        manaCosts = new float[] { 20.0f, 22.0f, 24.0f, 26.0f, 28.0f };
        effect = null;  // Assign appropriate ParticleSystem
        animationTrigger = "Skill2";
        key = KeyCode.W;
        level = 1;
        maxLevel = 5;
        evolvedSkill = null;
    }
}
