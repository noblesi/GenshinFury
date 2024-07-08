using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkillBook_UI : MonoBehaviour
{
    [SerializeField] private SkillLibrary skillLibrary; // 전체 스킬 라이브러리
    [SerializeField] private Transform skillGrid; // 스킬들을 배치할 그리드
    [SerializeField] private GameObject skillButtonPrefab; // 스킬 버튼 프리팹
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
        // 스킬 버튼 클릭 시 필요한 동작을 정의
        if (playerSkillManager.CanAffordSkill(skill))
        {
            playerSkillManager.UnlockSkill(skill);
        }
    }
}
