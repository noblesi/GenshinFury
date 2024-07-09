using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ItemPickup : MonoBehaviour
{
    public float PickUpRadius = 1f;
    public InventoryItemData itemData;

    private SphereCollider myCollider;

    private void Awake()
    {
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = PickUpRadius;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Collision detected with {other.gameObject.name}");

        var inventory = other.transform.GetComponent<InventoryHolder>();

        if (!inventory)
        {
            Debug.Log("No InventoryHolder found on collided object.");
            return;
        }

        Debug.Log("InventoryHolder found. Attempting to add item to inventory.");

        if (inventory.InventorySystem.AddToInventory(itemData, 1))
        {
            Debug.Log("Item successfully added to inventory.");
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("Failed to add item to inventory.");
        }
    }
}
