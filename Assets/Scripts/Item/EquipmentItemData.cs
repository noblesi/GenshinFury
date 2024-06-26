using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Item", menuName = "Items/EquipmentItem")]
public class EquipmentItemData : ItemData
{
    public EquipmentType EquipmentType => _equipmentType;
    public int AttackPower => _attackPower;
    public int DefensePower => _defensePower;
    public int Agility => _agility;
    public int Intelligence => _intelligence;

    [SerializeField] private EquipmentType _equipmentType;
    [SerializeField] private int _attackPower;
    [SerializeField] private int _defensePower;
    [SerializeField] private int _agility;
    [SerializeField] private int _intelligence;

    public override Item CreateItem()
    {
        return new EquipmentItem(this);
    }
}
