using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WizardSkills", menuName = "PlayerClass/Wizard")]
public class WizardSkills : BasePlayerClass
{
    private void OnEnable()
    {
        className = "Wizard";
        skillBook = new List<SkillData>
        {
            Resources.Load<SkillData>("Skills/Wizard/Fireball"),
            Resources.Load<SkillData>("Skills/Wizard/Blizzard"),
            Resources.Load<SkillData>("Skills/Wizard/Curse"),
            Resources.Load<SkillData>("Skills/Wizard/MeteorShower")
        };
    }
}
