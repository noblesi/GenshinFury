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
    public int recoveryAmount; // 회복량
    public float buffDuration; // 버프 지속 시간 (초)

    // 소비 아이템 사용 시 효과 적용 메서드
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
