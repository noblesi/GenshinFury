using System.Collections.Generic;
using UnityEngine;

public class Archer : Player
{
    public List<SkillData> archerSkills = new List<SkillData>();

    protected override void Start()
    {
        base.Start();
        // �߰����� �ʱ�ȭ�� �ʿ��� ��� ���⿡ �߰�
    }

    public override void Initialize(PlayerData playerData)
    {
        base.Initialize(playerData);
        archerSkills = LoadSkillsFromLibraries(playerData.SkillLibrary);
    }
}
