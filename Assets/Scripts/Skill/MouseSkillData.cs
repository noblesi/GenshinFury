using UnityEngine;

public class MouseSkillData : MonoBehaviour
{
    public static MouseSkillData Instance { get; private set; }

    public SkillData AssignedSkillSlot { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateMouseSlot(SkillData skillData)
    {
        AssignedSkillSlot = skillData;
        // Update visual representation of the mouse slot (if any)
    }

    public void ClearSlot()
    {
        AssignedSkillSlot = null;
        // Clear visual representation of the mouse slot (if any)
    }
}
