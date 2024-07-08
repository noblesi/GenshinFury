using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkillBook_UI : MonoBehaviour
{
    [SerializeField] private SkillLibrary skillLibrary; // ��ü ��ų ���̺귯��
    [SerializeField] private Transform skillGrid; // ��ų���� ��ġ�� �׸���
    [SerializeField] private GameObject skillButtonPrefab; // ��ų ��ư ������
    private PlayerClass currentClass;
    private PlayerSkillManager playerSkillManager;

    public void Init(PlayerSkillManager skillManager, PlayerClass playerClass)
    {
        playerSkillManager = skillManager;
        currentClass = playerClass;
        PopulateSkills();
    }

    private void PopulateSkills()
    {
        ClearSkillGrid();

        var classSkills = skillLibrary.PlayerSkillLibrary.Where(skill => skill.RequiredClass == currentClass).ToList();

        foreach (var skill in classSkills)
        {
            var skillButton = Instantiate(skillButtonPrefab, skillGrid);
            skillButton.GetComponent<Image>().sprite = skill.SkillIcon;
            skillButton.transform.Find("Text").GetComponent<Text>().text = skill.SkillName;
            skillButton.GetComponent<Button>().onClick.AddListener(() => OnSkillButtonClick(skill));
        }
    }

    private void ClearSkillGrid()
    {
        foreach (Transform child in skillGrid)
        {
            Destroy(child.gameObject);
        }
    }

    private void OnSkillButtonClick(SkillData skill)
    {
        // ��ų ��ư Ŭ�� �� �ʿ��� ������ ����
        if (playerSkillManager.CanAffordSkill(skill))
        {
            playerSkillManager.UnlockSkill(skill);
        }
    }
}
