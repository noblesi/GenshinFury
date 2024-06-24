using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentDatabase", menuName = "Equipment/Database")]
public class EquipmentDatabase : ScriptableObject
{
    public List<EquipmentItem> equipmentItems;

    [Header("Drop Rates")]
    public float commonDropRate = 0.5f;
    public float rareDropRate = 0.3f;
    public float epicDropRate = 0.15f;
    public float uniqueDropRate = 0.04f;
    public float legendaryDropRate = 0.01f;

    [Header("General Drop Settings")]
    public float itemDropChance = 0.7f; // 아이템 드랍 확률

    public EquipmentItem GetRandomEquipment(EquipmentType equipmentType, WeaponType? weaponType = null)
    {
        List<EquipmentItem> filteredItems = equipmentItems.FindAll(item =>
            item.equipmentType == equipmentType &&
            (weaponType == null || item.weaponType == weaponType));

        if (filteredItems.Count > 0)
        {
            float totalRate = commonDropRate + rareDropRate + epicDropRate + uniqueDropRate + legendaryDropRate;
            float randomPoint = Random.value * totalRate;

            foreach (var item in filteredItems)
            {
                float dropRate = GetDropRate(item.rarity);

                if (randomPoint < dropRate)
                {
                    return item;
                }
                else
                {
                    randomPoint -= dropRate;
                }
            }
        }
        return null;
    }

    private float GetDropRate(ItemRarity rarity)
    {
        switch (rarity)
        {
            case ItemRarity.Common:
                return commonDropRate;
            case ItemRarity.Rare:
                return rareDropRate;
            case ItemRarity.Epic:
                return epicDropRate;
            case ItemRarity.Unique:
                return uniqueDropRate;
            case ItemRarity.Legendary:
                return legendaryDropRate;
            default:
                return 0;
        }
    }
}
