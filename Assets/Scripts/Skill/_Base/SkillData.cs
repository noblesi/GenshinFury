using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/SkillData")]
public class SkillData : ScriptableObject
{
    public List<TypeData> TypeData;
    public string SkillName;
    public bool OverwriteDescription;
    [TextArea(1, 4)] public string SkillDescription;
    public Sprite SkillIcon;
    public List<SkillData> SkillPrerequisites = new List<SkillData>();
    public int RequiredLevel;
    public int UpgradeCost;
    public int SkillTier;
    public PlayerClass RequiredClass;
    public SkillType SkillType;
    public DamageType DamageType;
    public int BaseDamage;
    public int DamagePerLevel;
    public float[] Cooldowns;
    public int[] ManaCosts;
    public float Range;
    public float AreaRadius;
    public ParticleSystem Effect;
    public KeyCode Key;
    public string AnimationTrigger;
    public int SkillLevel;
    public int MaxSkillLevel;

    private void OnValidate()
    {
        if (TypeData.Count == 0) return;
        if (OverwriteDescription) return;
        if (SkillName == string.Empty) SkillName = name;

        GenerateDescription();
    }

    private void GenerateDescription()
    {
        if (SkillType == SkillType.Passive)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{SkillName} increases ");
            for (int i = 0; i < TypeData.Count; i++)
            {
                sb.Append(TypeData[i].StatType.ToString());
                sb.Append(" by ");
                sb.Append(TypeData[i].SkillIncreaseAmount.ToString());
                sb.Append(TypeData[i].IsPercentage ? "%" : " point(s)");
                if (i == TypeData.Count - 2) sb.Append(" and ");
                else sb.Append(i < TypeData.Count - 1 ? ", " : ".");
            }
            SkillDescription = sb.ToString();
        }
        else
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{SkillName} is an active skill that ");
            switch (SkillType)
            {
                case SkillType.Buff:
                    sb.Append("provides a buff.");
                    break;
                case SkillType.Debuff:
                    sb.Append("applies a debuff.");
                    break;
                case SkillType.Attack:
                    sb.Append($"deals {DamageType.ToString().ToLower()} damage.");
                    break;
            }
            SkillDescription = sb.ToString();
        }
    }

    public int GetDamage()
    {
        return BaseDamage + (SkillLevel - 1) * DamagePerLevel;
    }

    public float GetCooldown()
    {
        return Cooldowns[SkillLevel - 1];
    }

    public int GetManaCost()
    {
        return ManaCosts[SkillLevel - 1];
    }

    public void SkillLevelUp()
    {
        if (SkillLevel < MaxSkillLevel)
        {
            SkillLevel++;
        }
    }
}

[System.Serializable]
public class TypeData
{
    public StatType StatType;
    public int SkillIncreaseAmount;
    public bool IsPercentage;
}

public enum StatType
{
    None,
    Strength,
    Dexterity,
    Intelligence
}

public enum SkillType
{
    Passive,
    Buff,
    Debuff,
    Attack
}

public enum DamageType
{
    None,
    Physical,
    Magical,
    TrueDamage
}