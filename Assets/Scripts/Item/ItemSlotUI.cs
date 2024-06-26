using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [Tooltip("슬롯 내에서 아이콘과 슬롯 사이의 여백")]
    [SerializeField] private float padding = 1f;

    [Tooltip("아이템 아이콘 이미지")]
    [SerializeField] private Image iconImage;

    [Tooltip("아이템 개수 텍스트")]
    [SerializeField] private Text amountText;

    [Tooltip("슬롯이 포커스될 때 나타나는 하이라이트 이미지")]
    [SerializeField] private Image highlightImage;

    [Space]
    [Tooltip("하이라이트 이미지 알파 값")]
    [SerializeField] private float highlightAlpha = 0.5f;

    [Tooltip("하이라이트 소요시간")]
    [SerializeField] private float highlightFadeDuration = 0.2f;

    public int Index { get; private set; }

    public bool HasItem => iconImage.sprite != null;

    public bool IsAccessible => isAccessibleSlot && isAccessibleItem;

    public RectTransform SlotRect => slotRect;
    public RectTransform IconRect => iconRect;

    private InventoryUI inventoryUI;

    private RectTransform slotRect;
    private RectTransform iconRect;
    private RectTransform highlightRect;

    private GameObject iconGameObject;
    private GameObject textGameObject;
    private GameObject highlightGameObject;

    private Image slotImage;

    private float currentHighlightAlpha = 0f;

    private bool isAccessibleSlot = true;
    private bool isAccessibleItem = true;

    private static readonly Color InaccessibleSlotcolor = new Color(0.2f, 0.2f, 0.2f);
    private static readonly Color InaccessibleIconColor = new Color(0.5f, 0.5f, 0.5f);

    private void Awake()
    {
        InitComponents();
        InitValues();
    }

    private void InitComponents()
    {
        inventoryUI = GetComponentInParent<InventoryUI>();

        slotRect = GetComponent<RectTransform>();
        iconRect = iconImage.rectTransform;
        highlightRect = highlightImage.rectTransform;

        iconGameObject = iconRect.gameObject;
        textGameObject = amountText.gameObject;
        highlightGameObject = highlightImage.gameObject;

        slotImage = GetComponent<Image>();
    }

    private void InitValues()
    {
        iconRect.pivot = new Vector2(0.5f, 0.5f);
        IconRect.anchorMin = Vector2.zero;
        IconRect.anchorMax = Vector2.one;

        iconRect.offsetMin = Vector2.one * padding;
        iconRect.offsetMax = Vector2.one * padding;

        highlightRect.pivot = iconRect.pivot;
        highlightRect.anchorMin = iconRect.anchorMin;
        highlightRect.anchorMax = iconRect.anchorMax;
        highlightRect.offsetMin = iconRect.offsetMin;
        highlightRect.offsetMax = iconRect.offsetMax;

        iconImage.raycastTarget = false;
        highlightImage.raycastTarget = false;

        HideIcon();
        highlightGameObject.SetActive(false);
    }

    private void ShowIcon() => iconGameObject.SetActive(true);
    private void HideIcon() => iconGameObject.SetActive(false);

    private void ShowText() => textGameObject.SetActive(true);
    private void HideText() => textGameObject.SetActive(false);

    public void SetSlotIndex(int index) => Index = index;

    public void SetSlotAccessibleState(bool value)
    {
        if (isAccessibleSlot == value) return;

        if (value)
        {
            slotImage.color = Color.black;
        }
        else
        {
            slotImage.color = InaccessibleIconColor;
            HideIcon();
            HideText();
        }

        isAccessibleSlot = value;
    }

    public void SetItemAccessibleState(bool value)
    {
        if (isAccessibleItem == value) return;

        if (value)
        {
            iconImage.color = Color.white;
            amountText.color = Color.white;
        }
        else
        {
            iconImage.color = InaccessibleIconColor;
            amountText.color = InaccessibleIconColor;
        }

        isAccessibleItem = value;
    }

    public void SwapOrMoveIcon(ItemSlotUI other)
    {
        if (other == null) return;
        if (other == this) return;
        if (!this.IsAccessible) return;
        if (!other.IsAccessible) return;

        var temp = iconImage.sprite;

        if (other.HasItem) SetItem(other.iconImage.sprite);

        else RemoveItem();

        other.SetItem(temp);
    }

    public void SetItem(Sprite itemSprite)
    {
        if(itemSprite != null)
        {
            iconImage.sprite = itemSprite;
            ShowIcon();
        }
        else
        {
            RemoveItem();
        }
    }

    public void RemoveItem()
    {
        iconImage.sprite = null;
        HideIcon();
        HideText();
    }

    public void SetIconAlpha(float alpha)
    {
        iconImage.color = new Color(
            iconImage.color.r, iconImage.color.g, iconImage.color.b, alpha);
    }

    public void SetItemAmount(int amount)
    {
        if (HasItem && amount > 1)
            ShowText();
        else
            HideText();

        amountText.text = amount.ToString();
    }

    public void Highlight(bool show)
    {
        if (!this.IsAccessible) return;

        if (show)
            StartCoroutine(nameof(HighlightFadeInRoutine));
        else
            StartCoroutine(nameof(HighlightFadeOutRoutine));
    }

    public void SetHighlightOnTop(bool value)
    {
        if (value)
            highlightRect.SetAsLastSibling();
        else
            highlightRect.SetAsFirstSibling();
    }

    private IEnumerator HighlightFadeInRoutine()
    {
        StopCoroutine(nameof(HighlightFadeOutRoutine));
        highlightGameObject.SetActive(true);

        float unit = highlightAlpha / highlightFadeDuration;

        for(; currentHighlightAlpha <= highlightAlpha; currentHighlightAlpha += unit * Time.deltaTime)
        {
            highlightImage.color = new Color(
                highlightImage.color.r,
                highlightImage.color.g,
                highlightImage.color.b,
                currentHighlightAlpha
            );

            yield return null;
        }
    }

    private IEnumerator HighlightFadeOutRoutine()
    {
        StopCoroutine(nameof(HighlightFadeInRoutine));

        float unit = highlightAlpha / highlightFadeDuration;

        for (; currentHighlightAlpha <= highlightAlpha; currentHighlightAlpha -= unit * Time.deltaTime)
        {
            highlightImage.color = new Color(
                highlightImage.color.r,
                highlightImage.color.g,
                highlightImage.color.b,
                currentHighlightAlpha
            );

            yield return null;
        }

        highlightGameObject.SetActive(false);
    }
}
