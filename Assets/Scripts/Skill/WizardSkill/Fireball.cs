using UnityEngine;

[CreateAssetMenu(fileName = "Fireball", menuName = "Skill/Fireball")]
public class Fireball : SkillData
{
    private void OnEnable()
    {
        skillName = "Fireball";
        types = new SkillType[] { SkillType.Ranged, SkillType.SingleTarget };
        damageType = DamageType.Magical;
        baseDamage = 1.2f;
        damagePerLevel = 0.3f;
        cooldowns = new float[] { 6.0f, 5.5f, 5.0f, 4.5f, 4.0f };
        manaCosts = new float[] { 15.0f, 18.0f, 21.0f, 24.0f, 27.0f };
        effect = null;  // Assign appropriate ParticleSystem
        animationTrigger = "Skill1";
        key = KeyCode.Q;
        level = 1;
        maxLevel = 5;
        evolvedSkill = Resources.Load<SkillData>("Data/SkillData/Inferno");
    }
}
