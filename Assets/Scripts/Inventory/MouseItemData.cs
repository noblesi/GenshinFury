using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class MouseItemData : MonoBehaviour
{
    public static MouseItemData Instance { get; private set; }

    public Image ItemSprite;
    public Text ItemCount;
    public InventorySlot AssignedInventorySlot { get; private set; } = new InventorySlot();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        ItemSprite.color = Color.clear;
        ItemCount.text = string.Empty;
    }

    public void UpdateMouseSlot(InventoryItemData itemData, int count)
    {
        AssignedInventorySlot.UpdateInventorySlot(itemData, count);
        ItemSprite.sprite = itemData.Icon;
        ItemCount.text = count > 1 ? count.ToString() : string.Empty;
        ItemSprite.color = Color.white;
    }

    private void Update()
    {
        if (AssignedInventorySlot.ItemData != null)
        {
            transform.position = Mouse.current.position.ReadValue();

            if (Mouse.current.leftButton.wasPressedThisFrame && !IsPointerOverUIObject())
            {
                ClearSlot();
            }
        }
    }

    public void ClearSlot()
    {
        AssignedInventorySlot.ClearSlot();
        ItemCount.text = string.Empty;
        ItemSprite.color = Color.clear;
        ItemSprite.sprite = null;
    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = Mouse.current.position.ReadValue()
        };
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}