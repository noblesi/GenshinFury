using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InventorySystem
{
    [SerializeField] private List<InventorySlot> inventorySlots;

    public List<InventorySlot> InventorySlots => inventorySlots;
    public int InventorySize => InventorySlots.Count;

    public UnityAction<InventorySlot> OnInventorySlotChanged;

    public InventorySystem(int size)
    {
        inventorySlots = new List<InventorySlot>(size);
        for (int i = 0; i < size; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
    }

    public bool AddToInventory(InventoryItemData itemToAdd, int amountToAdd)
    {
        if (itemToAdd == null) throw new System.ArgumentNullException(nameof(itemToAdd));
        if (amountToAdd <= 0) throw new System.ArgumentOutOfRangeException(nameof(amountToAdd));

        if (ContainsItem(itemToAdd, out List<InventorySlot> inventorySlot))
        {
            foreach (var slot in inventorySlot)
            {
                if (slot.RoomLeftInStack(amountToAdd))
                {
                    slot.AddToStack(amountToAdd);
                    OnInventorySlotChanged?.Invoke(slot);
                    return true;
                }
            }
        }

        if (HasFreeSlot(out InventorySlot freeSlot))
        {
            freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
            OnInventorySlotChanged?.Invoke(freeSlot);
            return true;
        }

        return false;
    }

    public bool ContainsItem(InventoryItemData itemToAdd, out List<InventorySlot> inventorySlot)
    {
        inventorySlot = new List<InventorySlot>();

        foreach (var slot in InventorySlots)
        {
            if (slot.ItemData == itemToAdd)
            {
                inventorySlot.Add(slot);
            }
        }

        return inventorySlot.Count > 0;
    }

    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        foreach (var slot in InventorySlots)
        {
            if (slot.ItemData == null)
            {
                freeSlot = slot;
                return true;
            }
        }

        freeSlot = null;
        return false;
    }
}
