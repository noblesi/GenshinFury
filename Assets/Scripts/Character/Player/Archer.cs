using System.Collections.Generic;
using UnityEngine;

public class Archer : Player
{
    public List<SkillData> archerSkills = new List<SkillData>();

    public override void Initialize()
    {
        currentClass = Resources.Load<BasePlayerClass>("Data/PlayerClass/ArcherSkills");
        archerSkills = new List<SkillData>(Resources.LoadAll<SkillData>("Data/Skills/Archer"));
        base.Initialize();
    }

    public override void HandleSkillInput()
    {
        base.HandleSkillInput();

        foreach (var skill in archerSkills)
        {
            if (Input.GetKeyDown(skill.key) && skillCooldownTimers[skill.key] <= 0)
            {
                UseSkill(skill);
            }
        }
    }
}
