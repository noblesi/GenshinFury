// Scripts/PlayerInteraction.cs
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 2.0f; // �÷��̾�� ������ ���� ��ȣ�ۿ� �Ÿ�
    public LayerMask itemLayer; // �������� �ִ� ���̾� ����
    public KeyCode interactKey = KeyCode.F; // ��ȣ�ۿ� Ű�� F�� ����

    public Text interactionText; // UI�� ������ ������ ǥ���ϴ� �ؽ�Ʈ

    private Inventory playerInventory;
    private ItemPickup currentItemPickup;

    private void Start()
    {
        playerInventory = GetComponent<Inventory>();
        interactionText.gameObject.SetActive(false); // UI �ؽ�Ʈ ��Ȱ��ȭ�� ����
    }

    private void Update()
    {
        CheckForNearbyItems();

        if (Input.GetKeyDown(interactKey) && currentItemPickup != null)
        {
            PickUpItem();
        }
    }

    private void CheckForNearbyItems()
    {
        Collider[] itemsInRange = Physics.OverlapSphere(transform.position, interactionRange, itemLayer);

        if (itemsInRange.Length > 0)
        {
            ItemPickup itemPickup = itemsInRange[0].GetComponent<ItemPickup>();
            if (itemPickup != null)
            {
                currentItemPickup = itemPickup;
                interactionText.text = "Press F to pick up " + itemPickup.item.itemName;
                interactionText.gameObject.SetActive(true);
            }
        }
        else
        {
            currentItemPickup = null;
            interactionText.gameObject.SetActive(false);
        }
    }

    private void PickUpItem()
    {
        if (currentItemPickup != null)
        {
            playerInventory.AddItem(currentItemPickup.item, currentItemPickup.quantity);
            Destroy(currentItemPickup.gameObject); // �������� ȹ�� �� ������Ʈ�� �����մϴ�.
            currentItemPickup = null;
            interactionText.gameObject.SetActive(false);
        }
    }
}
