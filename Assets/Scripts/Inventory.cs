using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public int inventorySize = 20; // 인벤토리 슬롯 개수
    public List<InventorySlot> slots = new List<InventorySlot>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeSlots();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSlots()
    {
        slots = new List<InventorySlot>();
        for (int i = 0; i < inventorySize; i++)
        {
            slots.Add(new InventorySlot());
        }
    }

    public bool AddItem(Item item, int amount = 1)
    {
        foreach (var slot in slots)
        {
            if (slot.item == item && slot.amount < item.maxStackSize)
            {
                slot.amount += amount;
                return true;
            }
        }

        foreach (var slot in slots)
        {
            if (slot.item == null)
            {
                slot.item = item;
                slot.amount = amount;
                return true;
            }
        }

        Debug.Log("Inventory is full");
        return false;
    }

    public bool RemoveItem(Item item, int amount = 1)
    {
        foreach (var slot in slots)
        {
            if (slot.item == item)
            {
                slot.amount -= amount;
                if (slot.amount <= 0)
                {
                    slot.item = null;
                    slot.amount = 0;
                }
                return true;
            }
        }

        Debug.Log("Item not found in inventory");
        return false;
    }
}

[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int amount;
}
