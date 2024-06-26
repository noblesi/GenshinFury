using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int Capacity { get; private set; }

    [SerializeField, Range(8, 64)]
    private int initialCapacity = 32;

    [SerializeField, Range(8, 64)]
    private int maxCapacity = 64;

    [SerializeField]
    private InventoryUI inventoryUI;

    [SerializeField]
    private Item[] items;

    private readonly HashSet<int> indexSetForUpdate = new HashSet<int>();

    private readonly static Dictionary<Type, int> sortWeightDictionary = new Dictionary<Type, int>
    {
        {typeof(PotionItemData), 10000 },
        {typeof(WeaponItemData), 20000 },
        {typeof(ArmorItemData), 30000 }
    };

    private class ItemComparer : IComparer<Item>
    {
        public int Compare(Item x, Item y)
        {
            return (x.Data.ID + sortWeightDictionary[x.Data.GetType()])
                - (y.Data.ID + sortWeightDictionary[y.Data.GetType()]);
        }
    }

    private static readonly ItemComparer itemComparer = new ItemComparer();

    private void Awake()
    {
        items = new Item[maxCapacity];
        Capacity = initialCapacity;
        this.inventoryUI.SetInventoryReference(this);
    }

    private void Start()
    {
        UpdateAccessibleStatesAll();
    }

    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < Capacity; 
    }

    private int FindEmptySlotIndex(int startIndex = 0)
    {
        for(int i = startIndex; i < Capacity; i++)
        {
            if (items[i] == null) return i;
        }
        return -1;
    }

    private int FindCountableItemSlotIndex(CountableItemData target, int startIndex = 0)
    {
        for (int i = startIndex; i < Capacity; i++)
        {
            var current = items[i];
            if (current == null)
                continue;

            // 아이템 종류 일치, 개수 여유 확인
            if (current.Data == target && current is CountableItem ci)
            {
                if (!ci.IsMax)
                    return i;
            }
        }

        return -1;
    }

    public bool HasItem(int index)
    {
        return IsValidIndex(index) && items[index] != null;
    }

    public bool IsCountableItem(int index)
    {
        return HasItem(index) && items[index] is CountableItem;
    }

    public int GetCurrentAmount(int index)
    {
        if (!IsValidIndex(index)) return -1;
        if (items[index] == null) return 0;

        CountableItem countableItem = items[index] as CountableItem;
        if (countableItem == null)
            return 1;

        return countableItem.Amount;
    }

    public ItemData GetItemData(int index)
    {
        if(!IsValidIndex(index)) return null;
        if (items[index] == null) return null;

        return items[index].Data;
    }

    public string GetItemName(int index)
    {
        if(!IsValidIndex(index)) return string.Empty;
        if (items[index] == null) return string.Empty;

        return items[index].Data.Name;
    }

    public void ConnectUI(InventoryUI inventoryUI)
    {
        this.inventoryUI = inventoryUI;
        this.inventoryUI.SetInventoryReference(this);
    }

    public int Add(ItemData itemData, int amount = 1)
    {
        int index;

        if (itemData is CountableItemData countableItemData)
        {
            bool findNextCountable = true;
            index = -1;

            while (amount > 0)
            {
                if (findNextCountable)
                {
                    index = FindCountableItemSlotIndex(countableItemData, index + 1);

                    if (index == -1)
                    {
                        findNextCountable = false;
                    }
                    else
                    {
                        CountableItem countableItem = items[index] as CountableItem;
                        amount = countableItem.AddAmountAndGetExcess(amount);

                        UpdateSlot(index);
                    }
                }
                else
                {
                    index = FindEmptySlotIndex(index + 1);

                    if (index == -1) break;

                    else
                    {
                        CountableItem countableItem = countableItemData.CreateItem() as CountableItem;
                        countableItem.SetAmount(amount);

                        items[index] = countableItem;

                        amount = (amount > countableItemData.MaxAmount) ? (amount - countableItemData.MaxAmount) : 0;

                        UpdateSlot(index);
                    }
                }
            }
        }

        else
        {
            if (amount == 1)
            {
                index = FindEmptySlotIndex();
                if (index != -1)
                {
                    items[index] = itemData.CreateItem();
                    amount = 0;

                    UpdateSlot(index);
                }
            }

            index = -1;
            for (; amount > 0; amount--)
            {
                index = FindEmptySlotIndex(index + 1);

                if (index != -1) break;

                items[index] = itemData.CreateItem();

                UpdateSlot(index);
            }
        }
        return amount;
    }

    public void UpdateSlot(int index)
    {
        if (!IsValidIndex(index)) return;

        Item item = items[index];

        if(item != null)
        {
            inventoryUI.SetItemIcon(index, item.Data.IconSprite);

            if(item is CountableItem countableItem)
            {
                if (countableItem.IsEmpty)
                {
                    items[index] = null;
                    RemoveIcon();
                    return;
                }
                else
                {
                    inventoryUI.SetItemAmountText(index, countableItem.Amount);
                }
            }
            else
            {
                RemoveIcon();
            }

            void RemoveIcon()
            {
                inventoryUI.RemoveItem(index);
                inventoryUI.HideItemAmountText(index);
            }
        }
    }

    private void UpdateSlot(params int[] indices)
    {
        foreach(var i in indices)
        {
            UpdateSlot(i);
        }
    }

    private void UpdateAllSlot()
    {
        for(int i = 0; i < Capacity; i++)
        {
            UpdateSlot(i);
        }
    }

    public void Remove(int index)
    {
        if (!IsValidIndex(index)) return;

        items[index] = null;
        UpdateSlot(index);
    }

    public void Swap(int index1, int index2)
    {
        if (!IsValidIndex(index1)) return;
        if (!IsValidIndex(index2)) return;

        Item item1 = items[index1];
        Item item2 = items[index2];

        if(item1 != null && item2 != null && 
            item1.Data == item2.Data &&
            item1 is CountableItem countableItem1 && item2 is CountableItem countableItem2)
        {
            int maxAmount = countableItem2.MaxAmount;
            int sum = countableItem1.Amount + countableItem2.Amount;

            if(sum <= maxAmount)
            {
                countableItem1.SetAmount(0);
                countableItem2.SetAmount(sum);
            }
            else
            {
                countableItem1.SetAmount(sum - maxAmount);
                countableItem2.SetAmount(maxAmount);
            }
        }
        else
        {
            items[index1] = item1;
            items[index2] = item2;
        }

        UpdateSlot(index1, index2);
    }

    public void SeparateAmount(int index1, int index2, int amount)
    {
        if (!IsValidIndex(index1)) return;
        if (!IsValidIndex(index2)) return;

        Item item1 = items[index1];
        Item item2 = items[index2];

        CountableItem countableItem1 = item1 as CountableItem;

        if (countableItem1 != null && item2 == null)
        {
            items[index2] = countableItem1.SeperateAndClone(amount);

            UpdateSlot(index1, index2);
        }
    }

    public void Use(int index)
    {
        if (items[index] == null) return;

        if (items[index] is IUsableItem usableItem)
        {
            bool succeeded = usableItem.Use();

            if(succeeded)
            {
                UpdateSlot(index);
            }
        }
    }

    public void UpdateAccessibleStatesAll()
    {
        inventoryUI.SetAccessibleSlotRange(Capacity);
    }

    public void TrimAll()
    {
        indexSetForUpdate.Clear();

        int i = -1;
        while (items[++i] != null) ;
        int j = i;

        while (true)
        {
            while (++j < Capacity && items[j] == null) ;

            if (j == Capacity) break;

            indexSetForUpdate.Add(i);
            indexSetForUpdate.Add(j);

            items[i] = items[j];
            items[j] = null;
            i++;
        }

        foreach(var index in indexSetForUpdate)
        {
            UpdateSlot(index);
        }

        inventoryUI.UpdateAllSlotFilters();
    }

    public void SortAll()
    {
        int i = -1;
        while (items[++i] != null) ;
        int j = i;

        while (true)
        {
            while (++j < Capacity && items[j] == null) ;

            if (j == Capacity) break;

            indexSetForUpdate.Add(i);
            indexSetForUpdate.Add(j);

            items[i] = items[j];
            items[j] = null;
            i++;
        }

        Array.Sort(items, 0, i, itemComparer);

        UpdateAllSlot();
        inventoryUI.UpdateAllSlotFilters();
    }
}
