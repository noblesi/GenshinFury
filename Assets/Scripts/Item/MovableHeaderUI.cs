using UnityEngine;
using UnityEngine.EventSystems;

public class MovableHeaderUI : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField] private Transform targetTransform;

    private Vector2 startPoint;
    private Vector2 moveStart;

    private void Awake()
    {
        if (targetTransform == null)
            targetTransform = transform.parent;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        startPoint = targetTransform.position;
        moveStart = eventData.position;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        targetTransform.position = startPoint + (eventData.position - moveStart);
    }
}
