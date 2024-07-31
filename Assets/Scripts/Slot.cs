using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private int index;
    private InventoryUI inventoryUI;

    public void Initialize(int index, InventoryUI inventoryUI)
    {
        this.index = index;
        this.inventoryUI = inventoryUI;
        Debug.Log($"Slot initialized with index: {index}");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (index >= 0 && index < inventoryUI.inventory.slots.Count)
        {
            InventorySlot slot = inventoryUI.inventory.slots[index];
            if (slot.item != null)
            {
                inventoryUI.ShowTooltip(slot.item);
            }
        }
        else
        {
            Debug.LogError($"Slot index {index} is out of range.");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryUI.HideTooltip();
    }
}
