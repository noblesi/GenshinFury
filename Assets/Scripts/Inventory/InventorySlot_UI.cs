using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot_UI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private Text itemCount;
    [SerializeField] private InventorySlot assignedInventorySlot;

    private Button button;

    public static InventorySlot_UI DraggedFromSlot { get; set; } // Changed to public set

    public InventorySlot AssignedInventorySlot => assignedInventorySlot;
    public InventoryDisplay ParentDisplay { get; private set; }

    private void Awake()
    {
        ClearSlot();

        button = GetComponent<Button>();
        button?.onClick.AddListener(OnUISlotClick);

        ParentDisplay = transform.parent.GetComponent<InventoryDisplay>();
    }

    public void Init(InventorySlot slot)
    {
        assignedInventorySlot = slot;
        UpdateUISlot(slot);
    }

    public void UpdateUISlot(InventorySlot slot)
    {
        if (slot.ItemData != null)
        {
            itemSprite.sprite = slot.ItemData.Icon;
            itemSprite.color = Color.white;

            if (slot.StackSize > 1) itemCount.text = slot.StackSize.ToString();
            else itemCount.text = string.Empty;
        }
        else
        {
            ClearSlot();
        }
    }

    public void UpdateUISlot()
    {
        if (assignedInventorySlot != null) UpdateUISlot(assignedInventorySlot);
    }

    public void ClearSlot()
    {
        assignedInventorySlot?.ClearSlot();
        itemSprite.sprite = null;
        itemSprite.color = Color.clear;
        itemCount.text = string.Empty;
    }

    public void OnUISlotClick()
    {
        ParentDisplay?.SlotClicked(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (assignedInventorySlot.ItemData != null)
        {
            MouseItemData.Instance.UpdateMouseSlot(assignedInventorySlot.ItemData, assignedInventorySlot.StackSize);
            ClearSlot();
            DraggedFromSlot = this; // Store the original slot
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
                var quickSlotUI = result.gameObject.GetComponent<QuickSlot_UI>();

                if (slotUI != null)
                {
                    slotUI.AssignedInventorySlot.AssignItem(MouseItemData.Instance.AssignedInventorySlot);
                    slotUI.UpdateUISlot();
                    MouseItemData.Instance.ClearSlot();
                    return;
                }
                else if (quickSlotUI != null)
                {
                    if (MouseItemData.Instance.AssignedInventorySlot.ItemData.Type == ItemType.Consumable)
                    {
                        quickSlotUI.OnDrop(eventData);
                    }
                    else
                    {
                        quickSlotUI.ReturnItemToOriginalInventorySlot();
                        MouseItemData.Instance.ClearSlot();
                    }
                    return;
                }
            }
            ReturnItemToOriginalInventorySlot();
            MouseItemData.Instance.ClearSlot();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (MouseItemData.Instance.AssignedInventorySlot.ItemData != null)
        {
            AssignedInventorySlot.AssignItem(MouseItemData.Instance.AssignedInventorySlot);
            UpdateUISlot();
            MouseItemData.Instance.ClearSlot();
        }
    }

    public void ReturnItemToOriginalInventorySlot()
    {
        if (DraggedFromSlot != null)
        {
            DraggedFromSlot.AssignedInventorySlot.AssignItem(MouseItemData.Instance.AssignedInventorySlot);
            DraggedFromSlot.UpdateUISlot();
            DraggedFromSlot = null;
        }
    }
}
