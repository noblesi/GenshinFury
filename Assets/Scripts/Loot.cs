using UnityEngine;

public class Loot : MonoBehaviour
{
    public Item item;
    public int amount = 1;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.PickUpItem(item, amount);
            Destroy(gameObject);
        }
    }
}
