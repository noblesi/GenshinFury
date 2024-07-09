using UnityEngine;

[CreateAssetMenu(menuName = "InventorySystem/Consumable Item")]
public class ConsumableItemData : InventoryItemData
{
    public enum ConsumableType
    {
        HealthRecovery,
        ManaRecovery,
        Buff
    }

    public ConsumableType consumableType;
    public int recoveryAmount; // ȸ����
    public float buffDuration; // ���� ���� �ð� (��)

    // �Һ� ������ ��� �� ȿ�� ���� �޼���
    public void ApplyEffect(Player player)
    {
        switch (consumableType)
        {
            case ConsumableType.HealthRecovery:
                player.ChangeHealth(player.PlayerData.PlayerStats.currentHealth + recoveryAmount);
                break;
            case ConsumableType.ManaRecovery:
                player.ChangeMana(player.PlayerData.PlayerStats.currentMana + recoveryAmount);
                break;
            case ConsumableType.Buff:
                player.ApplyBuff(this);
                break;
        }
    }
}
