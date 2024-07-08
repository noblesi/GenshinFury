using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillQuickSlot_UI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private Image skillIcon;
    [SerializeField] private Text skillName;
    [SerializeField] private SkillQuickSlot assignedSkillQuickSlot;

    public SkillQuickSlot AssignedSkillQuickSlot => assignedSkillQuickSlot;

    private void Awake()
    {
        ClearSlot();
    }

    public void Init(SkillQuickSlot slot)
    {
        assignedSkillQuickSlot = slot;
        UpdateUISlot(slot);
    }

    public void UpdateUISlot(SkillQuickSlot slot)
    {
        if (slot.SkillData != null)
        {
            skillIcon.sprite = slot.SkillData.SkillIcon;
            skillIcon.color = Color.white;
            skillName.text = slot.SkillData.SkillName;
        }
        else
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        skillIcon.sprite = null;
        skillIcon.color = Color.clear;
        skillName.text = string.Empty;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (MouseSkillData.Instance.AssignedSkillSlot != null)
        {
            assignedSkillQuickSlot.UpdateQuickSlot(MouseSkillData.Instance.AssignedSkillSlot);
            UpdateUISlot(assignedSkillQuickSlot);
            MouseSkillData.Instance.ClearSlot();
        }
        else
        {
            ReturnSkillToOriginalSlot();
            MouseSkillData.Instance.ClearSlot();
        }
    }

    public void ReturnSkillToOriginalSlot()
    {
        if (SkillSlot_UI.DraggedFromSlot != null)
        {
            SkillSlot_UI.DraggedFromSlot.AssignSkill(MouseSkillData.Instance.AssignedSkillSlot);
            SkillSlot_UI.DraggedFromSlot.UpdateUISlot();
            SkillSlot_UI.DraggedFromSlot = null;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (assignedSkillQuickSlot.SkillData != null)
        {
            MouseSkillData.Instance.UpdateMouseSlot(assignedSkillQuickSlot.SkillData);
            ClearSlot();
            SkillSlot_UI.DraggedFromSlot = null; // 퀵슬롯의 드래그 시작 시 슬롯 초기화
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (MouseSkillData.Instance.AssignedSkillSlot != null)
        {
            MouseSkillData.Instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (MouseSkillData.Instance.AssignedSkillSlot != null)
        {
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            foreach (var result in results)
            {
                var slotUI = result.gameObject.GetComponent<SkillSlot_UI>();

                if (slotUI != null)
                {
                    slotUI.AssignSkill(MouseSkillData.Instance.AssignedSkillSlot);
                    slotUI.UpdateUISlot();
                    MouseSkillData.Instance.ClearSlot();
                    return;
                }
            }
            // 스킬이 스킬 슬롯에 드롭되지 않은 경우, 원래의 퀵슬롯으로 반환
            assignedSkillQuickSlot.UpdateQuickSlot(MouseSkillData.Instance.AssignedSkillSlot);
            UpdateUISlot(assignedSkillQuickSlot);
            MouseSkillData.Instance.ClearSlot();
        }
    }
}
