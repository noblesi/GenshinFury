using UnityEngine;
using UnityEngine.UI;

public class SkillSlot_UI : MonoBehaviour
{
    [SerializeField] private Image skillIcon;
    [SerializeField] private Text skillName;
    public static SkillSlot_UI DraggedFromSlot { get; set; } // 추가된 속성
    public SkillData SkillData { get; private set; }

    public void Init(SkillData skillData)
    {
        SkillData = skillData;
        UpdateUISlot();
    }

    public void UpdateUISlot()
    {
        if (SkillData != null)
        {
            skillIcon.sprite = SkillData.SkillIcon;
            skillIcon.color = Color.white;
            skillName.text = SkillData.SkillName;
        }
        else
        {
            skillIcon.sprite = null;
            skillIcon.color = Color.clear;
            skillName.text = string.Empty;
        }
    }

    public void AssignSkill(SkillData skillData)
    {
        SkillData = skillData;
        UpdateUISlot();
    }
}
