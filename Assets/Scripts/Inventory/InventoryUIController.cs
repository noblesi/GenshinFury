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
        InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
    }

    private void OnDisable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory;
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
            var playerInventory = FindObjectOfType<Player>().GetComponent<InventoryHolder>().InventorySystem;
            DisplayInventory(playerInventory);
        }
    }

    private void DisplayInventory(InventorySystem inventoryToDisplay)
    {
        inventoryPanel.gameObject.SetActive(true);
        inventoryPanel.GetComponentInChildren<DynamicInventoryDisplay>().RefreshDynamicInventory(inventoryToDisplay);
    }
}
