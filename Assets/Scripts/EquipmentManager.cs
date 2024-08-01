using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;

    public Equipment[] currentEquipment;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
            currentEquipment = new Equipment[numSlots];
            DontDestroyOnLoad(gameObject);  // 싱글톤 인스턴스 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipSlot;
        Equipment oldItem = null;

        if (currentEquipment[slotIndex] != null)
        {
            oldItem = currentEquipment[slotIndex];
            Inventory.Instance.AddItem(oldItem, 1); // 'amount' 매개 변수를 추가
        }

        currentEquipment[slotIndex] = newItem;
        InventoryUI.Instance.UpdateUI(); // 인벤토리 UI 업데이트
    }
}
