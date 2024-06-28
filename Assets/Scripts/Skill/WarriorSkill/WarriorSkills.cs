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
            Resources.Load<SkillData>("Skills/Warrior/Slash"),
            Resources.Load<SkillData>("Skills/Warrior/Whirlwind"),
            Resources.Load<SkillData>("Skills/Warrior/BattleShout"),
            Resources.Load<SkillData>("Skills/Warrior/Berserk")
        };
    }
}
