using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WarriorSkills", menuName = "PlayerClass/Warrior")]
public class WarriorSkills : BasePlayerClass
{
    private void OnEnable()
    {
        className = "Warrior";
        skillBook = new List<SkillData>
        {
            Resources.Load<SkillData>("SkillData/Slash"),
            Resources.Load<SkillData>("SkillData/Whirlwind"),
            Resources.Load<SkillData>("SkillData/BattleShout"),
            Resources.Load<SkillData>("SkillData/Berserk")
        };
    }
}
