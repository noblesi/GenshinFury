using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public int inventorySize = 20;
    public List<InventorySlot> slots = new List<InventorySlot>();

    public event Action OnInventoryChanged;  // UI 업데이트를 위한 이벤트

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // 싱글톤 인스턴스 유지
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

    public void AddItem(Item item, int amount)
    {
        foreach (var slot in slots)
        {
            if (slot.item == item && slot.amount < item.maxStackSize)
            {
                slot.amount += amount;
                OnInventoryChanged?.Invoke();  // 아이템 추가 시 UI 업데이트 트리거
                return;
            }
        }

        foreach (var slot in slots)
        {
            if (slot.item == null)
            {
                slot.item = item;
                slot.amount = amount;
                OnInventoryChanged?.Invoke();  // 아이템 추가 시 UI 업데이트 트리거
                return;
            }
        }

        Debug.Log("Inventory is full");
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
                OnInventoryChanged?.Invoke();  // 아이템 제거 시 UI 업데이트 트리거
                return true;
            }
        }

        Debug.Log("Item not found in inventory");
        return false;
    }
}
