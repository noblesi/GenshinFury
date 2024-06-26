using UnityEngine;

[CreateAssetMenu(fileName = "New Armor Item", menuName = "Items/ArmorItem")]
public class ArmorItemData : EquipmentItemData
{
    public override Item CreateItem()
    {
        return new ArmorItem(this);
    }
}
