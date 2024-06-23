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
            Resources.Load<SkillData>("SkillData/Fireball"),
            Resources.Load<SkillData>("SkillData/Blizzard")
        };
    }
}
