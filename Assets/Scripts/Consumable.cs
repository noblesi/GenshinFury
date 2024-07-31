using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Consumable")]
public class Consumable : Item
{
    public int healthRestore;
    public int manaRestore;

    public void Use(Player player)
    {
        if (healthRestore > 0)
        {
            player.Heal(healthRestore);
        }
        if (manaRestore > 0)
        {
            player.RestoreMana(manaRestore);
        }
    }
}
