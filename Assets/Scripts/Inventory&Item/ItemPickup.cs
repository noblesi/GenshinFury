// Scripts/ItemPickup.cs
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    public int quantity = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory playerInventory = other.GetComponent<Inventory>();
            if (playerInventory != null)
            {
                playerInventory.AddItem(item, quantity);
                Destroy(gameObject); // 아이템을 획득 후 오브젝트를 제거합니다.
            }
        }
    }
}
