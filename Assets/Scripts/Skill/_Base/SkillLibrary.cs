using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill Library", menuName = "Skill/Skill Library")]
public class SkillLibrary : ScriptableObject
{
    public List<SkillData> PlayerSkillLibrary;

    public List<SkillData> GetSkillsOfTier(int tier)
    {
        return PlayerSkillLibrary.Where(skill => skill.SkillTier == tier).ToList();
    }
}
