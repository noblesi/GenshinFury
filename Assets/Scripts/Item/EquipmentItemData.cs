using UnityEngine;

[CreateAssetMenu(menuName = "InventorySystem/Equipment Item")]
public class EquipmentItemData : InventoryItemData
{
    public enum EquipmentType
    {
        Helmet,
        Armor,
        Gloves,
        Boots,
        Weapon
    }

    public EquipmentType equipmentType;
    public int strengthBonus;
    public int dexterityBonus;
    public int intelligenceBonus;
}
