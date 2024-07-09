using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUIController : MonoBehaviour
{
    public GameObject inventoryPanel;

    private void Awake()
    {
        inventoryPanel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        InventoryHolder.OnInventoryChanged += UpdateInventoryUI;
    }

    private void OnDisable()
    {
        InventoryHolder.OnInventoryChanged -= UpdateInventoryUI;
    }

    private void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            ToggleInventory();
        }

        if (inventoryPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            inventoryPanel.gameObject.SetActive(false);
        }
    }

    private void ToggleInventory()
    {
        if (inventoryPanel.gameObject.activeInHierarchy)
        {
            inventoryPanel.gameObject.SetActive(false);
        }
        else
        {
            DisplayInventory();
        }
    }

    private void DisplayInventory()
    {
        var playerInventory = FindObjectOfType<Player>().GetComponent<InventoryHolder>().InventorySystem;
        inventoryPanel.gameObject.SetActive(true);
        inventoryPanel.GetComponentInChildren<DynamicInventoryDisplay>().RefreshDynamicInventory(playerInventory);
    }

    private void UpdateInventoryUI()
    {
        if (inventoryPanel.gameObject.activeInHierarchy)
        {
            var playerInventory = FindObjectOfType<Player>().GetComponent<InventoryHolder>().InventorySystem;
            inventoryPanel.GetComponentInChildren<DynamicInventoryDisplay>().RefreshDynamicInventory(playerInventory);
        }
    }
}
