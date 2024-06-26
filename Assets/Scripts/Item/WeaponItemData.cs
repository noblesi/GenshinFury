using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Item", menuName = "Items/WeaponItem")]
public class WeaponItemData : EquipmentItemData
{
    public WeaponType WeaponType => _weaponType;

    [SerializeField] private WeaponType _weaponType;

    public override Item CreateItem()
    {
        return new WeaponItem(this);
    }
}
