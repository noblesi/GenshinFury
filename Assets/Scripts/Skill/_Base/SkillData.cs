using UnityEngine;

public enum SkillType
{
    Melee,
    Ranged,
    SingleTarget,
    AreaOfEffect,
    Buff,
    Debuff
}

public enum DamageType
{
    Physical,
    Magical,
    True
}

[CreateAssetMenu(fileName = "SkillData", menuName = "Skill/SkillData")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public SkillType[] types;
    public DamageType damageType;
    public float[] cooldowns; // Cooldowns for each level
    public float baseDamage; // Base damage multiplier
    public float damagePerLevel; // Damage multiplier increase per level
    public ParticleSystem effect;
    public string animationTrigger;
    public KeyCode key;
    public int level;
    public int maxLevel;
    public SkillData evolvedSkill;

    public float GetCooldown()
    {
        if (level <= cooldowns.Length)
        {
            return cooldowns[level - 1];
        }
        return cooldowns[cooldowns.Length - 1];
    }

    public float GetDamage()
    {
        return baseDamage + (damagePerLevel * (level - 1));
    }
}
