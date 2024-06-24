using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerClass", menuName = "PlayerClass/BasePlayerClass")]
public class BasePlayerClass : ScriptableObject
{
    public string className;
    public List<SkillData> skillBook;
    public int skillPoints;
    public PlayerStats stats;

    public void AddSkillPoint()
    {
        skillPoints++;
    }

    public bool LevelUpSkill(string skillName)
    {
        SkillData skill = skillBook.Find(s => s.skillName == skillName);
        if (skill != null && skill.level < skill.maxLevel && skillPoints > 0)
        {
            skill.level++;
            skillPoints--;

            // Handle skill evolution
            if (skill.level == skill.maxLevel && skill.evolvedSkill != null)
            {
                skillBook.Add(skill.evolvedSkill);
            }

            return true;
        }
        return false;
    }

    public void ApplyLevelUp(ref int health, ref int mana, ref int strength, ref int agility, ref int intelligence)
    {
        health += stats.healthPerLevel;
        mana += stats.manaPerLevel;
        strength += stats.strengthPerLevel;
        agility += stats.agilityPerLevel;
        intelligence += stats.intelligencePerLevel;
    }
}
