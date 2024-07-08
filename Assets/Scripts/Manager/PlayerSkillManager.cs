using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSkillManager : MonoBehaviour
{
    private PlayerStats playerStats;
    private int skillPoints;
    private int playerLevel;
    private Dictionary<KeyCode, float> skillCooldownTimers = new Dictionary<KeyCode, float>();

    public int Strength => playerStats.strength;
    public int Dexterity => playerStats.dexterity;
    public int Intelligence => playerStats.intelligence;
    public int SkillPoints => skillPoints;
    public int PlayerLevel => playerLevel;

    public UnityAction OnSkillPointsChanged;
    public UnityAction OnPlayerLevelChanged;

    private List<SkillData> unlockedSkills = new List<SkillData>();

    private void Update()
    {
        UpdateSkillCooldowns();
    }

    public void SetPlayerStats(PlayerStats stats)
    {
        playerStats = stats;
        skillPoints = stats.skillPoints;
        playerLevel = stats.playerLevel;
    }

    public void SetSkills(List<SkillData> skills)
    {
        unlockedSkills = skills;
        InitializeSkillCooldowns(skills);
    }

    private void InitializeSkillCooldowns(List<SkillData> skills)
    {
        foreach (var skill in skills)
        {
            if (!skillCooldownTimers.ContainsKey(skill.Key))
            {
                skillCooldownTimers[skill.Key] = 0f;
            }
        }
    }

    private void UpdateSkillCooldowns()
    {
        var keys = new List<KeyCode>(skillCooldownTimers.Keys);
        foreach (var key in keys)
        {
            if (skillCooldownTimers[key] > 0)
            {
                skillCooldownTimers[key] -= Time.deltaTime;
            }
        }
    }

    public float GetSkillCooldown(KeyCode key)
    {
        if (skillCooldownTimers.ContainsKey(key))
        {
            return skillCooldownTimers[key];
        }
        return 0f;
    }

    public void TriggerSkillCooldown(SkillData skill)
    {
        if (skillCooldownTimers.ContainsKey(skill.Key))
        {
            skillCooldownTimers[skill.Key] = skill.GetCooldown();
        }
    }

    public void GainSkillPoint()
    {
        skillPoints++;
        OnSkillPointsChanged?.Invoke();
    }

    public bool CanAffordSkill(SkillData skill)
    {
        return skillPoints >= skill.UpgradeCost;
    }

    public void UnlockSkill(SkillData skill)
    {
        if (!CanAffordSkill(skill)) return;

        ModifyStats(skill);
        unlockedSkills.Add(skill);
        skillPoints -= skill.UpgradeCost;
        OnSkillPointsChanged?.Invoke();
    }

    private void ModifyStats(SkillData skill)
    {
        foreach (TypeData data in skill.TypeData)
        {
            switch (data.StatType)
            {
                case StatType.Strength:
                    ModifyStat(ref playerStats.strength, data);
                    break;
                case StatType.Dexterity:
                    ModifyStat(ref playerStats.dexterity, data);
                    break;
                case StatType.Intelligence:
                    ModifyStat(ref playerStats.intelligence, data);
                    break;
            }
        }
    }

    public bool IsSkillUnlocked(SkillData skill)
    {
        return unlockedSkills.Contains(skill);
    }

    public bool PreReqsMet(SkillData skill)
    {
        return skill.SkillPrerequisites.Count == 0 || skill.SkillPrerequisites.All(unlockedSkills.Contains);
    }

    private void ModifyStat(ref int stat, TypeData data)
    {
        bool isPercent = data.IsPercentage;

        if (isPercent) stat += (int)(stat * (data.SkillIncreaseAmount / 100f));
        else stat += data.SkillIncreaseAmount;
    }
}
