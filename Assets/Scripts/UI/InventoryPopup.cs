using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopup : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPopup;
    //[SerializeField] private List<ItemData> items = new List<ItemData>();
    [SerializeField] private Transform itemSlotParent;
    [SerializeField] private GameObject itemSlotPrefab;

    private bool isActive = false;

    //private void Start()
    //{
    //    inventoryPopup.SetActive(isActive);
    //    RefreshInventoryUI();
    //}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        isActive = !isActive;
        inventoryPopup.SetActive(isActive);
    }

    //public void Trim()
    //{
    //    items = items.Where(item => item != null).ToList();
    //    RefreshInventoryUI();
    //}

    //public void Sort()
    //{
    //    items.Sort((a,b) => a.Name.CompareTo(b.Name));
    //    RefreshInventoryUI();
    //}

    //private void RefreshInventoryUI()
    //{
    //    foreach(Transform child in itemSlotParent)
    //    {
    //        Destroy(child.gameObject);
    //    }

    //    foreach(var item in items)
    //    {
    //        GameObject itemSlot = Instantiate(itemSlotPrefab, itemSlotParent);
    //        Image itemIcon = itemSlot.transform.Find("ItemIcon").GetComponent<Image>();
    //        itemIcon.sprite = item.IconSprite;
    //    }
    //}
}
