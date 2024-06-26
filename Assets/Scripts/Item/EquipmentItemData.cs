using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Item", menuName = "Items/EquipmentItem")]
public class EquipmentItemData : ItemData
{
    public EquipmentType EquipmentType => _equipmentType;
    public int BaseHealth => _baseHealth;
    public int BaseDefense => _baseDefense;
    public int BaseAttack => _baseAttack;
    public int BaseAgility => _baseAgility; // �߰��� �Ӽ�
    public int BaseIntelligence => _baseIntelligence; // �߰��� �Ӽ�
    public EquipmentRarity Rarity => _rarity;

    [SerializeField] private EquipmentType _equipmentType;
    [SerializeField] private int _baseHealth;
    [SerializeField] private int _baseDefense;
    [SerializeField] private int _baseAttack;
    [SerializeField] private int _baseAgility; // �߰��� �ʵ�
    [SerializeField] private int _baseIntelligence; // �߰��� �ʵ�
    [SerializeField] private EquipmentRarity _rarity;
    [SerializeField] private List<RandomStat> _randomStats;

    public override Item CreateItem()
    {
        return new EquipmentItem(this);
    }

    public List<RandomStat> GenerateRandomStats()
    {
        int numStats = Random.Range(1, 4); // ������ �߰� ���� ��
        List<RandomStat> randomStats = new List<RandomStat>();

        for (int i = 0; i < numStats; i++)
        {
            RandomStat randomStat = _randomStats[Random.Range(0, _randomStats.Count)];
            RandomStat newStat = new RandomStat
            {
                StatType = randomStat.StatType,
                MinValue = randomStat.MinValue,
                MaxValue = randomStat.MaxValue,
                Value = Random.Range(randomStat.MinValue, randomStat.MaxValue) // ������ �� ����
            };
            randomStats.Add(newStat);
        }

        return randomStats;
    }
}

[System.Serializable]
public class RandomStat
{
    public StatType StatType;
    public int MinValue;
    public int MaxValue;
    public int Value;
}

public enum StatType
{
    Strength,
    Agility,
    Intelligence,
    Health,
    Mana
}
