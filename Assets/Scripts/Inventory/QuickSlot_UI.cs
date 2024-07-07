using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickSlot_UI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private Text itemCount;
    [SerializeField] private QuickSlot assignedQuickSlot;

    public QuickSlot AssignedQuickSlot => assignedQuickSlot;

    private void Awake()
    {
        ClearSlot();
    }

    public void Init(QuickSlot slot)
    {
        assignedQuickSlot = slot;
        UpdateUISlot(slot);
    }

    public void UpdateUISlot(QuickSlot slot)
    {
        if (slot.ItemData != null)
        {
            itemSprite.sprite = slot.ItemData.Icon;
            itemSprite.color = Color.white;

            if (slot.ItemCount > 1) itemCount.text = slot.ItemCount.ToString();
            else itemCount.text = string.Empty;
        }
        else
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        itemSprite.sprite = null;
        itemSprite.color = Color.clear;
        itemCount.text = string.Empty;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (MouseItemData.Instance.AssignedInventorySlot.ItemData != null)
        {
            if (MouseItemData.Instance.AssignedInventorySlot.ItemData.Type == ItemType.Consumable)
            {
                assignedQuickSlot.UpdateQuickSlot(MouseItemData.Instance.AssignedInventorySlot.ItemData, MouseItemData.Instance.AssignedInventorySlot.StackSize);
                UpdateUISlot(assignedQuickSlot);
                MouseItemData.Instance.ClearSlot();
            }
            else
            {
                Debug.LogWarning("Only consumable items can be assigned to quick slots.");
                ReturnItemToOriginalInventorySlot();
                MouseItemData.Instance.ClearSlot();
            }
        }
    }

    public void ReturnItemToOriginalInventorySlot()
    {
        if (InventorySlot_UI.DraggedFromSlot != null)
        {
            InventorySlot_UI.DraggedFromSlot.AssignedInventorySlot.AssignItem(MouseItemData.Instance.AssignedInventorySlot);
            InventorySlot_UI.DraggedFromSlot.UpdateUISlot();
            InventorySlot_UI.DraggedFromSlot = null;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (assignedQuickSlot.ItemData != null)
        {
            MouseItemData.Instance.UpdateMouseSlot(assignedQuickSlot.ItemData, assignedQuickSlot.ItemCount);
            ClearSlot();
            InventorySlot_UI.DraggedFromSlot = null; // Reset the dragged from slot for quick slot
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (MouseItemData.Instance.AssignedInventorySlot.ItemData != null)
        {
            MouseItemData.Instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (MouseItemData.Instance.AssignedInventorySlot.ItemData != null)
        {
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            foreach (var result in results)
            {
                var slotUI = result.gameObject.GetComponent<InventorySlot_UI>();

                if (slotUI != null)
                {
                    slotUI.AssignedInventorySlot.AssignItem(MouseItemData.Instance.AssignedInventorySlot);
                    slotUI.UpdateUISlot();
                    MouseItemData.Instance.ClearSlot();
                    return;
                }
            }
            // If the item is not dropped on an inventory slot, return it to the original quick slot
            assignedQuickSlot.UpdateQuickSlot(MouseItemData.Instance.AssignedInventorySlot.ItemData, MouseItemData.Instance.AssignedInventorySlot.StackSize);
            UpdateUISlot(assignedQuickSlot);
            MouseItemData.Instance.ClearSlot();
        }
    }
}
