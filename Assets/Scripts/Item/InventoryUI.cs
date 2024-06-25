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

    [Header("Connected Objects")]
    [SerializeField] private RectTransform contentAreaRectTransform;
    [SerializeField] private GameObject slotUIPrefab;

    private GraphicRaycaster graphicRaycaster;
    private PointerEventData pointerEventData;
    private List<RaycastResult> raycastResultList;

    private ItemSlotUI startDragSlot;
    private Transform startDragIconTransform;

    private Vector3 startDragIconPoint;
    private Vector3 startDragCursorPoint;
    private int startDragSlotSiblingIndex;

    private List<ItemSlotUI> slotUIList = new List<ItemSlotUI>();

    private ItemSlotUI pointerOverSlot;

    private void Update()
    {
        pointerEventData.position = Input.mousePosition;

        OnPointerEnterAndExit();
        OnPointerDown();
        OnPointerDrag();
        OnPointerUp();
    }

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

    private void InitSlots()
    {
        slotUIPrefab.TryGetComponent(out RectTransform slotRect);
        slotRect.sizeDelta = new Vector2(slotSize, slotSize);

        slotUIPrefab.TryGetComponent(out ItemSlotUI itemSlot);
        if(itemSlot == null)
            slotUIPrefab.AddComponent<ItemSlotUI>();

        slotUIPrefab.SetActive(false);

        Vector2 startPos = new Vector2(contentAreaPadding, -contentAreaPadding);
        Vector2 currentPos = startPos;

        slotUIList = new List<ItemSlotUI>(verticalSlotCount * horizontalSlotCount);

        for(int j = 0; j < verticalSlotCount; j++)
        {
            for(int i = 0; i < horizontalSlotCount; i++)
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

        if(slotUIPrefab.scene.rootCount != 0)
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

    private void EndDrag()
    {
        ItemSlotUI endDragSlot = RaycastAndGetFirstComponent<ItemSlotUI>();

        if(endDragSlot != null && endDragSlot.IsAccessible)
        {
            TrySwapItems(startDragSlot, endDragSlot);
        }

        if (!IsOverUI())
        {
            TryRemoveItem(index);
        }
    }

    private void TrySwapItems(ItemSlotUI from, ItemSlotUI to)
    {
        if (from == to) return;

        from.SwapOrMoveIcon(to);
        Inventory.Swap(from.Index, to.Index);
    }

    private void TryRemoveItem(int index)
    {
        Inventory.Remove(index);
    }

    private bool IsOverUI()
        => EventSystem.current.IsPointerOverGameObject();

    public void SetAccessibleSlotRange(int accessibleSlotCount)
    {
        for (int i = 0; i < slotUIList.Count; i++)
        {
            slotUIList[i].SetSlotAccessibleState(i < accessibleSlotCount);
        }
    }
}
