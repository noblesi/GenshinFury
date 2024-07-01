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
            Resources.Load<SkillData>("Data/Skills/Archer/ArrowShot"),
            Resources.Load<SkillData>("Data/Skills/Archer/RainOfArrows"),
            Resources.Load<SkillData>("Data/Skills/Archer/Haste"),
            Resources.Load<SkillData>("Data/Skills/Archer/PiercingArrow")
        };
    }
}
