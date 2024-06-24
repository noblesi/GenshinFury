using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/SkillData")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public SkillCategory skillCategory; // 스킬 대분류 추가
    public SkillType[] types;
    public DamageType damageType;
    public float baseDamage;
    public float damagePerLevel;
    public float[] cooldowns;
    public float[] manaCosts;
    public float range;
    public float areaRadius;
    public ParticleSystem effect;
    public string animationTrigger;
    public KeyCode key;
    public int level;
    public int maxLevel;
    public SkillData evolvedSkill;

    public float GetDamage()
    {
        return baseDamage + (level - 1) * damagePerLevel;
    }

    public float GetCooldown()
    {
        return cooldowns[level - 1];
    }

    public float GetManaCost()
    {
        return manaCosts[level - 1];
    }
}
