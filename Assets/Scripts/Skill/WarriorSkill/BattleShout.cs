using UnityEngine;

[CreateAssetMenu(fileName = "BattleShout", menuName = "Skill/BattleShout")]
public class BattleShout : SkillData
{
    private void OnEnable()
    {
        skillName = "Battle Shout";
        types = new SkillType[] { SkillType.Buff };
        damageType = DamageType.Physical;
        baseDamage = 0.0f; // Buff doesn't deal damage
        damagePerLevel = 0.0f;
        cooldowns = new float[] { 20.0f, 18.0f, 16.0f, 14.0f, 12.0f };
        manaCosts = new float[] { 30.0f, 32.0f, 34.0f, 36.0f, 38.0f };
        effect = null;  // Assign appropriate ParticleSystem
        animationTrigger = "Skill3";
        key = KeyCode.E;
        level = 1;
        maxLevel = 5;
        evolvedSkill = null;
    }
}
