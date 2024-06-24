using UnityEngine;

[CreateAssetMenu(fileName = "ArrowShot", menuName = "Skill/ArrowShot")]
public class ArrowShot : SkillData
{
    private void OnEnable()
    {
        skillName = "Arrow Shot";
        types = new SkillType[] { SkillType.Ranged, SkillType.SingleTarget };
        damageType = DamageType.Physical;
        baseDamage = 1.0f;
        damagePerLevel = 0.2f;
        cooldowns = new float[] { 4.0f, 3.5f, 3.0f, 2.5f, 2.0f };
        manaCosts = new float[] { 10.0f, 12.0f, 14.0f, 16.0f, 18.0f };
        effect = null;  // Assign appropriate ParticleSystem
        animationTrigger = "Skill1";
        key = KeyCode.Q;
        level = 1;
        maxLevel = 5;
        evolvedSkill = Resources.Load<SkillData>("SkillData/PowerArrowShot");
    }
}
