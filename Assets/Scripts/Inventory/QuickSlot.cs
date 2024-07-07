using UnityEngine;

[System.Serializable]
public class QuickSlot
{
    [SerializeField] private InventoryItemData itemData;
    [SerializeField] private int itemCount;

    public InventoryItemData ItemData => itemData;
    public int ItemCount => itemCount;

    public void UpdateQuickSlot(InventoryItemData data, int count)
    {
        if (data.Type == ItemType.Consumable)
        {
            itemData = data;
            itemCount = count;
        }
        else
        {
            Debug.LogWarning("Only consumable items can be assigned to quick slots.");
        }
    }

    public void ReduceItemCount()
    {
        if (itemCount > 0)
        {
            itemCount--;
            if (itemCount == 0)
            {
                ClearSlot();
            }
        }
    }

    public void ClearSlot()
    {
        itemData = null;
        itemCount = 0;
    }
}
