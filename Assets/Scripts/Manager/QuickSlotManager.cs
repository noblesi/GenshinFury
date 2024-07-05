using UnityEngine;
using UnityEngine.UI;

public class QuickSlotsManager : MonoBehaviour
{
    public Image[] itemSlots; // 소비 아이템 슬롯 이미지 배열
    public Image[] skillSlots; // 스킬 슬롯 이미지 배열

    private Player player;

    //private void Start()
    //{
    //    player = FindObjectOfType<Player>();
    //    if (player == null)
    //    {
    //        Debug.LogError("Player object not found.");
    //        return;
    //    }

    //    UpdateQuickSlots();
    //    player.OnSkillsChanged += UpdateQuickSlots;
    //}

    //private void UpdateQuickSlots()
    //{
    //    if (player != null)
    //    {
    //        for (int i = 0; i < itemSlots.Length; i++)
    //        {
    //            if (player.quickItemSlots[i] != null)
    //            {
    //                itemSlots[i].sprite = player.quickItemSlots[i].IconSprite;
    //                itemSlots[i].enabled = true;
    //            }
    //            else
    //            {
    //                itemSlots[i].enabled = false;
    //            }
    //        }

    //        for (int i = 0; i < skillSlots.Length; i++)
    //        {
    //            if (player.quickSkillSlots[i] != null)
    //            {
    //                skillSlots[i].sprite = player.quickSkillSlots[i].skillIcon;
    //                skillSlots[i].enabled = true;
    //            }
    //            else
    //            {
    //                skillSlots[i].enabled = false;
    //            }
    //        }
    //    }
    //}

    //private void OnDestroy()
    //{
    //    if (player != null)
    //    {
    //        player.OnSkillsChanged -= UpdateQuickSlots;
    //    }
    //}
}
