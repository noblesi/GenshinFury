using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Item", menuName = "Equipment/Item")]
public class EquipmentItem : ScriptableObject
{
    public string itemName;
    public EquipmentType equipmentType;
    public WeaponType weaponType; // Only applicable if equipmentType is Weapon
    public int attackPower;
    public int defensePower;
    public int agility;
    public int intelligence;
    public ItemRarity rarity; // ������ ��� �߰�
    public Sprite icon;
}
