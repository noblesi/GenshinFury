using System.Collections.Generic;
using UnityEngine;

public class Warrior : Player
{
    public List<SkillData> warriorSkills = new List<SkillData>();

    protected override void Start()
    {
        base.Start();
        // �߰����� �ʱ�ȭ�� �ʿ��� ��� ���⿡ �߰�
    }

    public override void Initialize(PlayerData playerData)
    {
        base.Initialize(playerData);
        warriorSkills = LoadSkillsFromLibraries(playerData.SkillLibrary);
    }
}
