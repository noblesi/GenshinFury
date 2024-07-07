using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuickSlotSystem
{
    [SerializeField] private List<QuickSlot> quickSlots;

    public List<QuickSlot> QuickSlots => quickSlots;
    public int QuickSlotSize => quickSlots.Count;

    public QuickSlotSystem(int size)
    {
        quickSlots = new List<QuickSlot>(size);
        for (int i = 0; i < size; i++)
        {
            quickSlots.Add(new QuickSlot());
        }
    }

    public void UpdateQuickSlot(int index, InventoryItemData itemData, int itemCount)
    {
        if (index >= 0 && index < QuickSlotSize)
        {
            quickSlots[index].UpdateQuickSlot(itemData, itemCount);
        }
    }

    public void ClearQuickSlot(int index)
    {
        if (index >= 0 && index < QuickSlotSize)
        {
            quickSlots[index].ClearSlot();
        }
    }
}
