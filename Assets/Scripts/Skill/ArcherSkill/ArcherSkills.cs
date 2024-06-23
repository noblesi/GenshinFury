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
            Resources.Load<SkillData>("SkillData/ArrowShot"),
            Resources.Load<SkillData>("SkillData/RainOfArrows")
        };
    }
}
