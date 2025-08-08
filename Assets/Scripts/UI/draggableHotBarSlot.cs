using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableHotBarSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 originalPosition;
    private Transform originalParent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.position;
        originalParent = transform.parent;
        canvasGroup.blocksRaycasts = false;
        transform.SetParent(canvas.transform); // Drag over UI
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        GameObject hovered = eventData.pointerEnter;
        HotbarSlot targetSlot = hovered ? hovered.GetComponentInParent<HotbarSlot>() : null;
        HotbarSlot originSlot = originalParent.GetComponent<HotbarSlot>();
        if (targetSlot != null && targetSlot != originSlot)
        {
            // Swap origin references
            var originRef = originSlot.originSlot;
            var targetRef = targetSlot.originSlot;

            originSlot.AssignReference(targetRef);
            targetSlot.AssignReference(originRef);
        }

        // Return to original layout position
        transform.SetParent(originalParent);
        rectTransform.position = originalPosition;
    }
}