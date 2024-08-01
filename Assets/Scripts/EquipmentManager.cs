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
            DontDestroyOnLoad(gameObject);  // �̱��� �ν��Ͻ� ����
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
            Inventory.Instance.AddItem(oldItem, 1); // 'amount' �Ű� ������ �߰�
        }

        currentEquipment[slotIndex] = newItem;
        InventoryUI.Instance.UpdateUI(); // �κ��丮 UI ������Ʈ
    }
}
