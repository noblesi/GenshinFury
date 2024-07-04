using UnityEngine;
using UnityEngine.UI;

public class SkillWindow : MonoBehaviour
{
    public Text skillPointsText; // ��ų ����Ʈ�� ǥ���� �ؽ�Ʈ
    public Image[] skillButtons; // ��ų ��ư �̹��� �迭
    public SkillData[] allSkills; // ��� ��ų �迭

    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        UpdateSkillWindow();
        UpdateSkillPoints();
    }

    private void UpdateSkillWindow()
    {
        for (int i = 0; i < skillButtons.Length; i++)
        {
            if (i < allSkills.Length)
            {
                skillButtons[i].sprite = allSkills[i].skillIcon;
                skillButtons[i].enabled = true;
            }
            else
            {
                skillButtons[i].enabled = false;
            }
        }
    }

    private void UpdateSkillPoints()
    {
        skillPointsText.text = $"Skill Points: {player.skillPoints}";
    }

    public void LevelUpSkill(int skillIndex)
    {
        if (skillIndex < allSkills.Length)
        {
            SkillData skill = allSkills[skillIndex];
            if (player.LevelUpSkill(skill.skillName))
            {
                UpdateSkillPoints();
            }
        }
    }
}
