using System.Collections.Generic;
using UnityEngine;

public class Wizard : Player
{
    public List<SkillData> wizardSkills = new List<SkillData>();

    public override void Initialize(GameData gameData)
    {
        currentClass = Resources.Load<BasePlayerClass>("PlayerClass/WizardSkills");
        wizardSkills = new List<SkillData>(Resources.LoadAll<SkillData>("Skills/Wizard"));
        base.Initialize(gameData);
    }

    public override void HandleSkillInput()
    {
        base.HandleSkillInput();

        foreach (var skill in wizardSkills)
        {
            if (Input.GetKeyDown(skill.key) && skillCooldownTimers[skill.key] <= 0)
            {
                UseSkill(skill);
            }
        }
    }
}
