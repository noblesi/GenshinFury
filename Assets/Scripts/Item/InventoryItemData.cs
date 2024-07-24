using UnityEngine;

//public enum ItemType
//{
//    Consumable,
//    Equipment,
//    Material
//}

[CreateAssetMenu(menuName = "InventorySystem/Inventory Item")]
public class InventoryItemData : ScriptableObject
{
    public int ID;
    public string DisplayName;
    [TextArea(4, 4)]
    public string Description;
    public Sprite Icon;
    public int MaxStackSize;
    public ItemType Type; // Add this line
}
