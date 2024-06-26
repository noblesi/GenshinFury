public class WeaponItem : EquipmentItem
{
    public WeaponType WeaponType { get; }

    public WeaponItem(WeaponItemData data) : base(data)
    {
        WeaponType = data.WeaponType;
    }
}
