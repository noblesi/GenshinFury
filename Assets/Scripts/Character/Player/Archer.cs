using System.Collections.Generic;
using UnityEngine;

public class Archer : Player
{
    public List<SkillData> archerSkills = new List<SkillData>();

    protected override void Start()
    {
        base.Start();
        // 추가적인 초기화가 필요한 경우 여기에 추가
    }

    public override void Initialize(PlayerData playerData)
    {
        base.Initialize(playerData);
        archerSkills = LoadSkillsFromLibraries(playerData.SkillLibrary);
    }
}
