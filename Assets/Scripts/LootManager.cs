using UnityEngine;

public class LootManager : MonoBehaviour
{
    public void DropItem(Vector3 position, ItemDrop itemDrop)
    {
        if (Random.Range(0f, 1f) <= itemDrop.dropChance)
        {
            int amount = Random.Range(itemDrop.minAmount, itemDrop.maxAmount + 1);
            for (int i = 0; i < amount; i++)
            {
                Vector3 dropPosition = position + new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
                GameObject drop = Instantiate(itemDrop.item.itemPrefab, dropPosition, Quaternion.identity);

                Equipment equipment = itemDrop.item as Equipment;
                if (equipment != null)
                {
                    equipment.AssignRandomStats();
                }

                ItemCollector itemCollector = drop.AddComponent<ItemCollector>();
                itemCollector.item = itemDrop.item;
                itemCollector.amount = 1;
            }
        }
    }
}
