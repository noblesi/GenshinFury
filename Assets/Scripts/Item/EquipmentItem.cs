using System.Collections.Generic;

public class EquipmentItem : Item
{
    public EquipmentType EquipmentType { get; }
    public int BaseHealth { get; }
    public int BaseDefense { get; }
    public int BaseAttack { get; }
    public int BaseAgility { get; } // 추가된 속성
    public int BaseIntelligence { get; } // 추가된 속성
    public EquipmentRarity Rarity { get; }
    public List<RandomStat> RandomStats { get; }

    public EquipmentItem(EquipmentItemData data) : base(data)
    {
        EquipmentType = data.EquipmentType;
        BaseHealth = data.BaseHealth;
        BaseDefense = data.BaseDefense;
        BaseAttack = data.BaseAttack;
        BaseAgility = data.BaseAgility; // 초기화
        BaseIntelligence = data.BaseIntelligence; // 초기화
        Rarity = data.Rarity;
        RandomStats = data.GenerateRandomStats();
    }

    public int GetStat(StatType statType)
    {
        int value = 0;
        foreach (var stat in RandomStats)
        {
            if (stat.StatType == statType)
            {
                value += stat.Value;
            }
        }
        return value;
    }
}
