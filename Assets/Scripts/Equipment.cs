using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentSlot equipSlot;
    public int strength;
    public int dexterity;
    public int intelligence;
    public int requiredLevel; // ���� ���� ����

    public void AssignRandomStats()
    {
        strength = Random.Range(1, 10) + (requiredLevel * 2); // ������ ���� ���� ��ȭ
        dexterity = Random.Range(1, 10) + (requiredLevel * 2);
        intelligence = Random.Range(1, 10) + (requiredLevel * 2);
    }
}

public enum EquipmentSlot { Head, Chest, Legs, Weapon, Shield, Feet }
