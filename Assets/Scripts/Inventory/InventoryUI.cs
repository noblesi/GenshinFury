using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InventoryUI : MonoBehaviour
{
    [Header("OPtions")]
    [Range(0, 10)]
    [SerializeField] private int horizontalSlotCount = 8;
    [Range(0, 10)]
    [SerializeField] private int verticalSlotCount = 8;
    [SerializeField] private float slotMargin = 8f;
    [SerializeField] private float contentAreaPadding = 20f;
    [Range(32, 64)]
    [SerializeField] private float slotSize = 64f;

    [Space]
    [SerializeField] private bool showTooltip = true;
    [SerializeField] private bool showHighlight = true;
    [SerializeField] private bool showRemovingPopup = true;

    [Header("Connected Objects")]
    [SerializeField] private RectTransform contentAreaRectTransform;
    [SerializeField] private GameObject slotUIPrefab;
    [SerializeField] private ItemTooltipUI itemTooltip;
    [SerializeField] private InventoryPopupUI popup;

    [Header("Buttons")]
    [SerializeField] private Button trimButton;
    [SerializeField] private Button sortButton;

    [Header("Filter Toggles")]
    [SerializeField] private Toggle toggleFilterAll;
    [SerializeField] private Toggle toggleFilterEquipments;
    [SerializeField] private Toggle toggleFilterPotions;

    [Space(16)]
    [SerializeField] private bool mouseReversed = false;

    private Inventory inventory;

    private List<ItemSlotUI> slotUIList = new List<ItemSlotUI>();

    private GraphicRaycaster graphicRaycaster;
    private PointerEventData pointerEventData;
    private List<RaycastResult> raycastResultList;

    private ItemSlotUI pointerOverSlot;
    private ItemSlotUI startDragSlot;
    private Transform startDragIconTransform;

    private int leftClick = 0;
    private int rightClick = 1;

    private Vector3 startDragIconPoint;
    private Vector3 startDragCursorPoint;
    private int startDragSlotSiblingIndex;

    private enum FilterOption
    {
        All, Equipment, Potion
    }

    private FilterOption currentFilterOption = FilterOption.All;

    private void Awake()
    {
        Init();
        InitSlots();
        InitButtonEvents();
        InitToggleEvents();
    }

    private void Update()
    {
        pointerEventData.position = Input.mousePosition;

        OnPointerEnterAndExit();
        if (showTooltip) ShowOrHideItemTooltip();
        OnPointerDown();
        OnPointerDrag();
        OnPointerUp();
    }

    private void Init()
    {
        TryGetComponent(out graphicRaycaster);
        if(graphicRaycaster == null)
            graphicRaycaster = gameObject.AddComponent<GraphicRaycaster>();

        pointerEventData = new PointerEventData(EventSystem.current);
        raycastResultList = new List<RaycastResult>(10);

        if(itemTooltip == null)
        {
            itemTooltip = GetComponentInChildren<ItemTooltipUI>();
        }
    }

    private void InitSlots()
    {
        slotUIPrefab.TryGetComponent(out RectTransform slotRect);
        slotRect.sizeDelta = new Vector2(slotSize, slotSize);

        slotUIPrefab.TryGetComponent(out ItemSlotUI itemSlot);
        if (itemSlot == null)
            slotUIPrefab.AddComponent<ItemSlotUI>();

        slotUIPrefab.SetActive(false);

        Vector2 startPos = new Vector2(contentAreaPadding, -contentAreaPadding);
        Vector2 currentPos = startPos;

        slotUIList = new List<ItemSlotUI>(verticalSlotCount * horizontalSlotCount);

        for (int j = 0; j < verticalSlotCount; j++)
        {
            for (int i = 0; i < horizontalSlotCount; i++)
            {
                int slotIndex = (horizontalSlotCount * j) + i;

                var slotRectTransform = CloneSlot();
                slotRectTransform.pivot = new Vector2(0f, 1f);
                slotRectTransform.anchoredPosition = currentPos;
                slotRectTransform.gameObject.SetActive(true);
                slotRectTransform.gameObject.name = $"Item Slot [{slotIndex}]";

                var slotUI = slotRectTransform.GetComponent<ItemSlotUI>();
                slotUI.SetSlotIndex(slotIndex);
                slotUIList.Add(slotUI);

                currentPos.x += (slotMargin + slotSize);
            }

            currentPos.x = startPos.x;
            currentPos.y -= (slotMargin + slotSize);
        }

        if (slotUIPrefab.scene.rootCount != 0)
        {
            Destroy(slotUIPrefab);
        }

        RectTransform CloneSlot()
        {
            GameObject slotGameObject = Instantiate(slotUIPrefab);
            RectTransform rt = slotGameObject.GetComponent<RectTransform>();
            rt.SetParent(contentAreaRectTransform);

            return rt;
        }
    }

    private void InitButtonEvents()
    {
        trimButton.onClick.AddListener(() => inventory.TrimAll());
        sortButton.onClick.AddListener(() => inventory.SortAll());
    }

    private void InitToggleEvents()
    {
        toggleFilterAll.onValueChanged.AddListener(flag => UpdateFilter(flag, FilterOption.All));
        toggleFilterEquipments.onValueChanged.AddListener(flag => UpdateFilter(flag, FilterOption.Equipment));
        toggleFilterPotions.onValueChanged.AddListener(flag => UpdateFilter(flag, FilterOption.Potion));

        void UpdateFilter(bool flag, FilterOption option)
        {
            if (flag)
            {
                currentFilterOption = option;
                UpdateAllSlotFilters();
            }
        }
    }

    private bool IsOverUI()
        => EventSystem.current.IsPointerOverGameObject();

    private T RaycastAndGetFirstComponent<T>() where T : Component
    {
        raycastResultList.Clear();

        graphicRaycaster.Raycast(pointerEventData, raycastResultList);

        if (raycastResultList.Count == 0) return null;

        return raycastResultList[0].gameObject.GetComponent<T>();
    }

    private void OnPointerEnterAndExit()
    {
        var previousSlot = pointerOverSlot;

        var currentSlot = pointerOverSlot = RaycastAndGetFirstComponent<ItemSlotUI>();

        if(previousSlot == null)
        {
            if(currentSlot != null)
            {
                OnCurrentEnter();
            }
        }
        else
        {
            if (currentSlot == null)
            {
                OnPreviousExit();
            }

            else if (previousSlot != currentSlot)
            {
                OnPreviousExit();
                OnCurrentEnter();
            }
        }

        void OnCurrentEnter()
        {
            currentSlot.Highlight(true);
        }

        void OnPreviousExit()
        {
            previousSlot.Highlight(false);
        }
    }

    private void ShowOrHideItemTooltip()
    {
        bool isValid =
            pointerOverSlot != null && pointerOverSlot.HasItem && pointerOverSlot.IsAccessible
            && (pointerOverSlot != startDragSlot);

        if (isValid)
        {
            UpdateTooltipUI(pointerOverSlot);
            itemTooltip.Show();
        }
        else
        {
            itemTooltip.Hide();
        }
    }

    private void OnPointerDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startDragSlot = RaycastAndGetFirstComponent<ItemSlotUI>();

            if(startDragSlot != null && startDragSlot.HasItem)
            {
                startDragIconTransform = startDragSlot.IconRect.transform;
                startDragIconPoint = startDragIconTransform.position;
                startDragCursorPoint = Input.mousePosition;

                startDragSlotSiblingIndex = startDragSlot.transform.GetSiblingIndex();
                startDragSlot.transform.SetAsLastSibling();

                startDragSlot.SetHighlightOnTop(false);
            }
            else
            {
                startDragSlot = null;
            }
        }
    }

    private void OnPointerDrag()
    {
        if (startDragSlot == null) return;

        if (Input.GetMouseButton(0))
        {
            startDragIconTransform.position = startDragIconPoint + (Input.mousePosition - startDragCursorPoint);
        }
    }

    private void OnPointerUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if(startDragSlot != null)
            {
                startDragIconTransform.position = startDragIconPoint;

                startDragSlot.transform.SetSiblingIndex(startDragSlotSiblingIndex);

                EndDrag();

                startDragSlot = null;
                startDragIconTransform = null;
            }
        }
    }

    

    private void EndDrag()
    {
        ItemSlotUI endDragSlot = RaycastAndGetFirstComponent<ItemSlotUI>();

        if(endDragSlot != null && endDragSlot.IsAccessible)
        {
            bool isSeparatable = 
                (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftShift) &&
                (inventory.IsCountableItem(startDragSlot.Index) && !inventory.HasItem(endDragSlot.Index)));

            bool isSeparation = false;
            int currentAmount = 0;

            if(isSeparatable)
            {
                currentAmount = inventory.GetCurrentAmount(startDragSlot.Index);
                if(currentAmount > 1)
                {
                    isSeparation = true;
                }
            }

            if (isSeparation)
                TrySeparateAmount(startDragSlot.Index, endDragSlot.Index, currentAmount);
            else
                TrySwapItems(startDragSlot, endDragSlot);

            UpdateTooltipUI(endDragSlot);
            return;
        }

        if (!IsOverUI())
        {
            int index = startDragSlot.Index;
            string itemName = inventory.GetItemName(index);
            int amount = inventory.GetCurrentAmount(index);

            if (amount > 1)
                itemName += $" x{amount}";


            popup.OpenConfirmationPopup(() => TryRemoveItem(index), itemName);
        }
    }

    private void TryRemoveItem(int index)
    {
        inventory.Remove(index);
    }

    private void TryUseItem(int index)
    {
        inventory.Use(index);
    }

    private void TrySwapItems(ItemSlotUI from, ItemSlotUI to)
    {
        if (from == to) return;

        from.SwapOrMoveIcon(to);

        inventory.Swap(from.Index, to.Index);
    }


    private void TrySeparateAmount(int index1, int index2, int amount)
    {
        if (index1 == index2) return;

        string itemName = inventory.GetItemName(index1);

        popup.OpenAmountInputPopup(
            amt => inventory.SeparateAmount(index1, index2, amt),
            amount, itemName
        );
    }

    private void UpdateTooltipUI(ItemSlotUI slot)
    {
        if(!slot.IsAccessible || !slot.HasItem) return;

        itemTooltip.SetItemInfo(inventory.GetItemData(slot.Index));

        itemTooltip.SetRectPosition(slot.SlotRect);
    }
    
    public void SetInventoryReference(Inventory inventory)
    {
        this.inventory = inventory;
    }

    public void InvertMouse(bool value)
    {
        leftClick = value ? 1 : 0;
        rightClick = value ? 0 : 1;

        mouseReversed = value;
    }

    public void SetItemIcon(int index, Sprite icon)
    {
        slotUIList[index].SetItem(icon);
    }

    public void SetItemAmountText(int index, int amount)
    {
        slotUIList[index].SetItemAmount(amount);
    }

    public void HideItemAmountText(int index)
    {
        slotUIList[index].SetItemAmount(1);
    }

    public void RemoveItem(int index)
    {
        slotUIList[index].RemoveItem();
    }

    public void SetAccessibleSlotRange(int accessibleSlotCount)
    {
        for (int i = 0; i < slotUIList.Count; i++)
        {
            slotUIList[i].SetSlotAccessibleState(i < accessibleSlotCount);
        }
    }


    public void UpdateSlotFilterState(int index, ItemData itemData)
    {
        bool isFiltered = true;

        if(itemData != null)
            switch (currentFilterOption)
            {
                case FilterOption.Equipment:
                    isFiltered = (itemData is EquipmentItemData);
                    break;
                case FilterOption.Potion:
                    isFiltered = (itemData is PotionItemData);
                    break;
            }

        slotUIList[index].SetItemAccessibleState(isFiltered);
    }

    public void UpdateAllSlotFilters()
    {
        int capacity = inventory.Capacity;

        for(int i = 0; i < capacity; i++)
        {
            ItemData data = inventory.GetItemData(i);
            UpdateSlotFilterState(i, data);
        }
    }
}
