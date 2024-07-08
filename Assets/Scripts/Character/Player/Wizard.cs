using System.Collections.Generic;
using UnityEngine;

public class Wizard : Player
{
    public List<SkillData> wizardSkills = new List<SkillData>();

    protected override void Start()
    {
        base.Start();
        // �߰����� �ʱ�ȭ�� �ʿ��� ��� ���⿡ �߰�
    }

    public override void Initialize(PlayerData playerData)
    {
        base.Initialize(playerData);
        wizardSkills = LoadSkillsFromLibraries(playerData.SkillLibrary);
    }
}
