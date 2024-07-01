using System.Collections.Generic;
using UnityEngine;

public class Warrior : Player
{
    public List<SkillData> warriorSkills = new List<SkillData>();

    public override void Initialize(GameData gameData)
    {
        currentClass = Resources.Load<BasePlayerClass>("Data/PlayerClass/WarriorSkills");
        warriorSkills = new List<SkillData>(Resources.LoadAll<SkillData>("Data/Skills/Warrior"));
        base.Initialize(gameData);
    }

    public override void HandleSkillInput()
    {
        base.HandleSkillInput();

        foreach (var skill in warriorSkills)
        {
            if (Input.GetKeyDown(skill.key) && skillCooldownTimers[skill.key] <= 0)
            {
                UseSkill(skill);
            }
        }
    }
}
