using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector2 offset;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform, eventData.position, eventData.pressEventCamera, out offset);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPointerPosition;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform, eventData.position, eventData.pressEventCamera, out localPointerPosition))
        {
            rectTransform.localPosition = localPointerPosition - offset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }
}
