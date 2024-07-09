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

        Debug.Log($"Trying to add {amountToAdd} of {itemToAdd.name}");

        if (ContainsItem(itemToAdd, out List<InventorySlot> inventorySlot))
        {
            foreach (var slot in inventorySlot)
            {
                if (slot.RoomLeftInStack(amountToAdd))
                {
                    slot.AddToStack(amountToAdd);
                    OnInventorySlotChanged?.Invoke(slot);
                    Debug.Log($"Updated existing slot with {amountToAdd} of {itemToAdd.name}");
                    return true;
                }
            }
        }

        if (HasFreeSlot(out InventorySlot freeSlot))
        {
            freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
            OnInventorySlotChanged?.Invoke(freeSlot);
            Debug.Log($"Added {amountToAdd} to new slot with {itemToAdd.name}");
            return true;
        }

        Debug.Log("No free slots available");
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
