using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    public GameObject slotsParent;  // Slots 오브젝트
    public GameObject slotPrefab;
    public Inventory inventory;
    public TooltipUI tooltip;

    private List<GameObject> slotsUI = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (inventory == null)
        {
            Debug.LogError("Inventory is not assigned in InventoryUI.");
        }
        else
        {
            Debug.Log("Inventory assigned successfully.");
        }

        if (slotsParent == null)
        {
            Debug.LogError("SlotsParent is not assigned in InventoryUI.");
        }
        else
        {
            Debug.Log("SlotsParent assigned successfully.");
        }

        if (slotPrefab == null)
        {
            Debug.LogError("SlotPrefab is not assigned in InventoryUI.");
        }
        else
        {
            Debug.Log("SlotPrefab assigned successfully.");
        }

        CreateSlots();
        UpdateUI();
    }

    private void CreateSlots()
    {
        if (inventory.slots == null)
        {
            Debug.LogError("Inventory slots are not initialized.");
            return;
        }

        Debug.Log($"Creating {inventory.inventorySize} slots.");
        for (int i = 0; i < inventory.inventorySize; i++)
        {
            GameObject slot = Instantiate(slotPrefab, slotsParent.transform);
            slotsUI.Add(slot);

            // Ensure Slot component is added to the slot prefab
            Slot slotComponent = slot.GetComponent<Slot>();
            if (slotComponent == null)
            {
                slotComponent = slot.AddComponent<Slot>();
            }
            slotComponent.Initialize(i, this);
        }
    }

    public void UpdateUI()
    {
        for (int i = 0; i < slotsUI.Count; i++)
        {
            if (i < inventory.slots.Count)
            {
                InventorySlot slot = inventory.slots[i];
                Image icon = slotsUI[i].transform.GetChild(1).GetComponent<Image>();
                Text amount = slotsUI[i].transform.GetChild(2).GetComponent<Text>();

                if (slot.item != null)
                {
                    icon.sprite = slot.item.icon;
                    icon.enabled = true;
                    amount.text = slot.amount > 1 ? slot.amount.ToString() : "";
                }
                else
                {
                    icon.enabled = false;
                    amount.text = "";
                }
            }
        }
    }

    public void ShowTooltip(Item item)
    {
        tooltip.ShowTooltip(item);
    }

    public void HideTooltip()
    {
        tooltip.HideTooltip();
    }

    private void OnSlotClick(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < inventory.slots.Count)
        {
            InventorySlot slot = inventory.slots[slotIndex];
            if (slot.item is Equipment equipment)
            {
                EquipmentManager.Instance.Equip(equipment);
            }
            else if (slot.item is Consumable consumable)
            {
                consumable.Use(Player.Instance); // Player.Instance를 사용하여 플레이어 참조
                inventory.RemoveItem(consumable, 1);
            }
            UpdateUI();
        }
        else
        {
            Debug.LogError("Slot index is out of range.");
        }
    }
}
