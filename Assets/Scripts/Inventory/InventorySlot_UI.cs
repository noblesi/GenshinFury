using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot_UI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private Text itemCount;
    private InventorySlot assignedInventorySlot;

    private Button button;

    public static InventorySlot_UI DraggedFromSlot { get; set; }

    public InventorySlot AssignedInventorySlot => assignedInventorySlot;
    public InventoryDisplay ParentDisplay { get; private set; }

    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
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
        if (assignedInventorySlot?.ItemData != null && MouseItemData.Instance != null)
        {
            MouseItemData.Instance.UpdateMouseSlot(assignedInventorySlot.ItemData, assignedInventorySlot.StackSize);
            ClearSlot();
            DraggedFromSlot = this;
            Debug.Log($"Begin Drag: {assignedInventorySlot.ItemData.name}, StackSize: {assignedInventorySlot.StackSize}");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (MouseItemData.Instance?.AssignedInventorySlot.ItemData != null)
        {
            MouseItemData.Instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (MouseItemData.Instance?.AssignedInventorySlot.ItemData != null)
        {
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            foreach (var result in results)
            {
                var slotUI = result.gameObject.GetComponent<InventorySlot_UI>();
                if (slotUI != null)
                {
                    if (slotUI != this)
                    {
                        SwapSlots(slotUI);
                    }
                    else
                    {
                        slotUI.AssignedInventorySlot.AssignItem(MouseItemData.Instance.AssignedInventorySlot);
                        slotUI.UpdateUISlot();
                    }

                    MouseItemData.Instance.ClearSlot();
                    Debug.Log($"End Drag: {slotUI.AssignedInventorySlot.ItemData?.name}, StackSize: {slotUI.AssignedInventorySlot.StackSize}");
                    return;
                }
            }
            ReturnItemToOriginalInventorySlot();
            MouseItemData.Instance.ClearSlot();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (MouseItemData.Instance?.AssignedInventorySlot.ItemData != null)
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

    private void SwapSlots(InventorySlot_UI targetSlot)
    {
        Debug.Log($"Swapping Slots: {assignedInventorySlot.ItemData?.name} <-> {targetSlot.AssignedInventorySlot.ItemData?.name}");

        var tempItemData = targetSlot.AssignedInventorySlot.ItemData;
        var tempStackSize = targetSlot.AssignedInventorySlot.StackSize;

        targetSlot.AssignedInventorySlot.UpdateInventorySlot(assignedInventorySlot.ItemData, assignedInventorySlot.StackSize);
        assignedInventorySlot.UpdateInventorySlot(tempItemData, tempStackSize);

        targetSlot.UpdateUISlot();
        UpdateUISlot();

        Debug.Log($"Swapped Slots: {assignedInventorySlot.ItemData?.name} <-> {targetSlot.AssignedInventorySlot.ItemData?.name}");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && assignedInventorySlot.ItemData is ConsumableItemData)
        {
            (assignedInventorySlot.ItemData as ConsumableItemData).ApplyEffect(player);
            assignedInventorySlot.RemoveFromStack(1);
            UpdateUISlot(assignedInventorySlot);
        }
    }
}
