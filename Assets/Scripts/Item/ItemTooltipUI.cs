using System;
using System.Collections;
using System.Collections.Generic;
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
}
