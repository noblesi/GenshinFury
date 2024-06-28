using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArcherSkills", menuName = "PlayerClass/Archer")]
public class ArcherSkills : BasePlayerClass
{
    private void OnEnable()
    {
        className = "Archer";
        skillBook = new List<SkillData>
        {
            Resources.Load<SkillData>("Skills/Archer/ArrowShot"),
            Resources.Load<SkillData>("Skills/Archer/RainOfArrows"),
            Resources.Load<SkillData>("Skills/Archer/Haste"),
            Resources.Load<SkillData>("Skills/Archer/PiercingArrow")
        };
    }
}
