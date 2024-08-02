using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<InventoryItem> items = new List<InventoryItem>();

    public void AddItem(Item item, int quantity = 1)
    {
        if (item.isStackable)
        {
            InventoryItem existingItem = items.Find(i => i.item.itemName == item.itemName);
            if (existingItem != null)
            {
                existingItem.quantity += quantity;
            }
            else
            {
                items.Add(new InventoryItem(item, quantity));
            }
        }
        else
        {
            items.Add(new InventoryItem(item, quantity));
        }
    }

    public void RemoveItem(Item item, int quantity = 1)
    {
        InventoryItem existingItem = items.Find(i => i.item.itemName == item.itemName);
        if (existingItem != null)
        {
            existingItem.quantity -= quantity;
            if (existingItem.quantity <= 0)
            {
                items.Remove(existingItem);
            }
        }
    }

    public List<InventoryItem> GetItems()
    {
        return items;
    }
}
