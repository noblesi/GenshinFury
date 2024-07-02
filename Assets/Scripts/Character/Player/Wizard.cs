using System.Collections.Generic;
using UnityEngine;

public class Wizard : Player
{
    public List<SkillData> wizardSkills = new List<SkillData>();

    public override void Initialize()
    {
        currentClass = Resources.Load<BasePlayerClass>("Data/PlayerClass/WizardSkills");
        wizardSkills = new List<SkillData>(Resources.LoadAll<SkillData>("Data/Skills/Wizard"));
        base.Initialize();
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
