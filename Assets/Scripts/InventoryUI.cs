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
            inventory.OnInventoryChanged += UpdateUI;  // 인벤토리 변경 시 UI 업데이트
        }

        CreateSlots();
        UpdateUI();  // 초기 UI 설정
    }

    private void CreateSlots()
    {
        if (inventory.slots == null)
        {
            Debug.LogError("Inventory slots are not initialized.");
            return;
        }

        for (int i = 0; i < inventory.inventorySize; i++)
        {
            GameObject slot = Instantiate(slotPrefab, slotsParent.transform);
            slotsUI.Add(slot);

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
}

[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int amount;
}