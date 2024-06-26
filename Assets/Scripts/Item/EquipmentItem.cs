using System.Collections.Generic;

public class EquipmentItem : Item
{
    public EquipmentType EquipmentType { get; }
    public int BaseHealth { get; }
    public int BaseDefense { get; }
    public int BaseAttack { get; }
    public int BaseAgility { get; } // �߰��� �Ӽ�
    public int BaseIntelligence { get; } // �߰��� �Ӽ�
    public EquipmentRarity Rarity { get; }
    public List<RandomStat> RandomStats { get; }

    public EquipmentItem(EquipmentItemData data) : base(data)
    {
        EquipmentType = data.EquipmentType;
        BaseHealth = data.BaseHealth;
        BaseDefense = data.BaseDefense;
        BaseAttack = data.BaseAttack;
        BaseAgility = data.BaseAgility; // �ʱ�ȭ
        BaseIntelligence = data.BaseIntelligence; // �ʱ�ȭ
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
