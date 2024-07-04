using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum QuickSlotType
{
    Item,
    Skill
}

public class QuickSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public QuickSlotType slotType;
    public Image icon;
    public int slotIndex;

    private Transform originalParent;
    private Vector2 originalPosition;
    private Canvas canvas;

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalPosition = transform.position;
        transform.SetParent(canvas.transform, true);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        GameObject target = eventData.pointerCurrentRaycast.gameObject;
        if (target != null && target.GetComponent<QuickSlot>() != null)
        {
            QuickSlot targetSlot = target.GetComponent<QuickSlot>();
            if (targetSlot.slotType == slotType)
            {
                Player player = FindObjectOfType<Player>();
                player.SwapQuickSlots(slotIndex, targetSlot.slotIndex, slotType);
            }
        }

        transform.SetParent(originalParent, true);
        transform.position = originalPosition;
    }

    public void UpdateIcon(Sprite newIcon)
    {
        if (newIcon != null)
        {
            icon.sprite = newIcon;
            icon.enabled = true;
        }
        else
        {
            icon.enabled = false;
        }
    }
}
