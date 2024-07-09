using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InventoryHolder : MonoBehaviour
{
    [SerializeField] private int inventorySize;
    [SerializeField] protected InventorySystem inventorySystem;

    public InventorySystem InventorySystem => inventorySystem;

    public static UnityAction OnInventoryChanged;
    public static UnityAction<InventorySystem> OnDynamicInventoryDisplayRequested;

    private void Awake()
    {
        inventorySystem = new InventorySystem(inventorySize);
    }

    private void OnEnable()
    {
        if (inventorySystem != null)
        {
            inventorySystem.OnInventorySlotChanged += HandleInventorySlotChanged;
        }
    }

    private void OnDisable()
    {
        if (inventorySystem != null)
        {
            inventorySystem.OnInventorySlotChanged -= HandleInventorySlotChanged;
        }
    }

    private void HandleInventorySlotChanged(InventorySlot slot)
    {
        Debug.Log($"Inventory slot changed: {slot.ItemData?.name}, {slot.StackSize}");
        OnInventoryChanged?.Invoke();
    }
}
