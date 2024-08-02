using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject slotPanel;
    public GameObject slotPrefab; // ½½·Ô ÇÁ¸®ÆÕ
    public int slotCount = 20; // ½½·ÔÀÇ °³¼ö
    public Inventory inventory;

    private void Start()
    {
        Debug.Log("InventoryUI: Start method called. Initializing slots.");
        InitializeSlots();
        UpdateInventoryUI();
    }

    private void InitializeSlots()
    {
        Debug.Log($"InventoryUI: Initializing {slotCount} slots.");
        for (int i = 0; i < slotCount; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotPanel.transform);
            Debug.Log($"InventoryUI: Slot {i + 1}/{slotCount} created. Slot Name: {newSlot.name}");
        }
    }

    public void UpdateInventoryUI()
    {
        // Get the direct child transforms of the slotPanel only (not their children)
        Transform[] slots = slotPanel.GetComponentsInChildren<Transform>(includeInactive: true);

        foreach (Transform slotTransform in slots)
        {
            if (slotTransform.parent == slotPanel.transform) // Only proceed if the transform is a direct child of slotPanel
            {
                Image itemIcon = slotTransform.Find("ItemIcon")?.GetComponent<Image>();
                Text itemQuantity = slotTransform.Find("ItemQuantity")?.GetComponent<Text>();

                if (itemIcon != null)
                {
                    itemIcon.sprite = null;
                }
                if (itemQuantity != null)
                {
                    itemQuantity.text = "";
                }
                Debug.Log($"InventoryUI: Slot {slotTransform.name} cleared.");
            }
        }

        // Populate the inventory items into the slots
        for (int i = 0; i < inventory.GetItems().Count; i++)
        {
            InventoryItem inventoryItem = inventory.GetItems()[i];
            Transform slotTransform = slots[i + 1]; // Assuming the slotPanel itself is the first element

            if (slotTransform.parent == slotPanel.transform)
            {
                Image itemIcon = slotTransform.Find("ItemIcon")?.GetComponent<Image>();
                Text itemQuantity = slotTransform.Find("ItemQuantity")?.GetComponent<Text>();

                if (itemIcon != null)
                {
                    itemIcon.sprite = inventoryItem.item.icon;
                }
                if (itemQuantity != null)
                {
                    itemQuantity.text = inventoryItem.quantity > 1 ? inventoryItem.quantity.ToString() : "";
                }
                Debug.Log($"InventoryUI: Placed {inventoryItem.item.itemName} in slot {slotTransform.name}. Quantity: {inventoryItem.quantity}");
            }
        }

        Debug.Log("InventoryUI: Inventory UI update complete.");
    }
}
