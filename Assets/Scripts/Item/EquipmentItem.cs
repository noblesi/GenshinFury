public class EquipmentItem : Item
{
    public EquipmentType EquipmentType { get; }
    public int AttackPower { get; }
    public int DefensePower { get; }
    public int Agility { get; }
    public int Intelligence { get; }

    public EquipmentItem(EquipmentItemData data) : base(data)
    {
        EquipmentType = data.EquipmentType;
        AttackPower = data.AttackPower;
        DefensePower = data.DefensePower;
        Agility = data.Agility;
        Intelligence = data.Intelligence;
    }
}
