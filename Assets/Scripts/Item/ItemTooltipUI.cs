using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltipUI : MonoBehaviour
{
    [SerializeField]
    private Text titleText;

    [SerializeField]
    private Text contentText;

    private RectTransform rectTransform;
    private CanvasScaler canvasScaler;

    private void Awake()
    {
        Init();
        Hide();
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);

    private void Init()
    {
        TryGetComponent(out rectTransform);
        rectTransform.pivot = new Vector2(0f, 1f);
        canvasScaler = GetComponent<CanvasScaler>();

        DisableAllChildrenRaycastTarget(transform);
    }

    private void DisableAllChildrenRaycastTarget(Transform transform)
    {
        transform.TryGetComponent(out Graphic graphic);
        if (graphic != null) graphic.raycastTarget = false;

        int childCount = transform.childCount;
        if (childCount == 0) return;

        for(int  i = 0; i < childCount; i++)
        {
            DisableAllChildrenRaycastTarget(transform.GetChild(i));
        }
    }

    public void SetItemInfo(ItemData data)
    {
        titleText.text = data.Name;
        contentText.text = data.Tooltip;
    }

    public void SetRectPosition(RectTransform slotRect)
    {
        float wRatio = Screen.width / canvasScaler.referenceResolution.x;
        float hRatio = Screen.height / canvasScaler.referenceResolution.y;
        float ratio =
            wRatio * (1f - canvasScaler.matchWidthOrHeight) +
            hRatio * (canvasScaler.matchWidthOrHeight);

        float slotWidth = slotRect.rect.width * ratio;
        float slotHeight = slotRect.rect.height * ratio;

        rectTransform.position = slotRect.position + new Vector3(slotWidth, -slotHeight);
        Vector2 pos = rectTransform.position;

        float width = rectTransform.rect.width * ratio;
        float height = rectTransform.rect.height * ratio;

        bool rightTruncated = pos.x + width > Screen.width;
        bool bottomTruncated = pos.y - height < 0f;

        ref bool R = ref rightTruncated;
        ref bool B = ref bottomTruncated;

        if(R && !B)
        {
            rectTransform.position = new Vector2(pos.x - width - slotWidth, pos.y);
        }
        else if(!R && B)
        {
            rectTransform.position = new Vector2(pos.x, pos.y + height + slotHeight);
        }
        else if(R && B)
        {
            rectTransform.position = new Vector2(pos.x - width - slotWidth, pos.y + height + slotHeight);
        }
    }
}
