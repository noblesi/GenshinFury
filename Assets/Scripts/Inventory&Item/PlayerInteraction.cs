// Scripts/PlayerInteraction.cs
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 2.0f; // 플레이어와 아이템 간의 상호작용 거리
    public LayerMask itemLayer; // 아이템이 있는 레이어 설정
    public KeyCode interactKey = KeyCode.F; // 상호작용 키를 F로 설정

    public Text interactionText; // UI에 아이템 정보를 표시하는 텍스트

    private Inventory playerInventory;
    private ItemPickup currentItemPickup;

    private void Start()
    {
        playerInventory = GetComponent<Inventory>();
        interactionText.gameObject.SetActive(false); // UI 텍스트 비활성화로 시작
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
            Destroy(currentItemPickup.gameObject); // 아이템을 획득 후 오브젝트를 제거합니다.
            currentItemPickup = null;
            interactionText.gameObject.SetActive(false);
        }
    }
}
