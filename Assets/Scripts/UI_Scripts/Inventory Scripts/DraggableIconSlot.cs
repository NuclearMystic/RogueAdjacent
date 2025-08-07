using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DraggableIconSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Image iconImage;
    public TextMeshProUGUI quantityLabel;
    public InventoryItem slotItem;
    public int quantity = 1;
    public Transform parentAfterDrag;
    public RectTransform crosshair;

    private DragGhostHandler dragGhostHandler;
    private Canvas mainUICanvas;
    private GameObject highlightOverlayInstance;
    private bool isDragging;
    private int dragAmount;
    private bool usedGhost;

    private void Start()
    {
        if (crosshair == null)
        {
            GameObject crosshairGO = GameObject.FindGameObjectWithTag("Crosshair");
            if (crosshairGO != null)
                crosshair = crosshairGO.GetComponent<RectTransform>();
        }

        if (mainUICanvas == null && UIManager.Instance != null)
        {
            mainUICanvas = UIManager.Instance.GetComponentInParent<Canvas>();
        }

        dragGhostHandler = UIManager.Instance.GhostPrefab;
    }

    private void Update()
    {
        if (!isDragging)
        {
            if (slotItem == null)
            {
                iconImage.color = new Color(0, 0, 0, 0);
                UpdateQuantity(0);
                Destroy(this.gameObject);
            }
            else
            {
                iconImage.color = Color.white;
                iconImage.sprite = slotItem.ObjectIcon;
            }
            if (quantity <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void SetQuantity(int amount)
    {
        quantity = amount;
        UpdateQuantity(quantity);
    }

    public void UpdateQuantity(int newAmount)
    {
        quantity = newAmount;
        if (quantityLabel != null)
            quantityLabel.text = quantity > 1 ? quantity.ToString() : "";
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (slotItem == null || dragGhostHandler == null) return;

        dragAmount = quantity;

        bool usingSlider = InventoryManager.Instance.selectedItemSlot == this &&
                           InventoryManager.Instance.qtySlider != null &&
                           InventoryManager.Instance.qtySlider.gameObject.activeInHierarchy;

        if (usingSlider)
        {
            dragAmount = Mathf.RoundToInt(InventoryManager.Instance.qtySlider.value);
        }

        var originSlot = GetComponentInParent<ItemSlot>();

        if (usingSlider || dragAmount < quantity)
        {
            quantity -= dragAmount;             
            UpdateQuantity(quantity);

            dragGhostHandler.BeginDrag(slotItem, dragAmount, originSlot);
            dragGhostHandler.gameObject.SetActive(true);
            eventData.pointerDrag = dragGhostHandler.gameObject;
            usedGhost = true;
        }
        else
        {
            parentAfterDrag = transform.parent;
            transform.SetParent(mainUICanvas.transform);
            transform.SetAsLastSibling();
            iconImage.raycastTarget = false;
            isDragging = true;
            usedGhost = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (usedGhost && dragGhostHandler != null && dragGhostHandler.isActiveAndEnabled)
        {
            dragGhostHandler.transform.position = crosshair != null ? crosshair.position : Input.mousePosition;
        }
        else
        {
            transform.position = crosshair != null ? crosshair.position : Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        iconImage.raycastTarget = true;

        if (usedGhost && dragGhostHandler != null && dragGhostHandler.isActiveAndEnabled)
        {
            dragGhostHandler.EndDrag();
            return;
        }

        GameObject hovered = eventData.pointerEnter;
        ItemSlot targetSlot = hovered ? hovered.GetComponentInParent<ItemSlot>() : null;

        if (targetSlot != null && targetSlot.CanAcceptItem(slotItem))
        {
            bool success = targetSlot.ReceiveInventoryItem(slotItem, quantity);
            if (success)
            {
                quantity = 0;
                UpdateQuantity(0);
                slotItem = null;
                transform.SetParent(parentAfterDrag);
                return;
            }
            else
            {
               // Debug.Log("Items failed to drop in slot in end drag");
            }
        }

        transform.localPosition = Vector3.zero;
        transform.SetParent(parentAfterDrag);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var manager = InventoryManager.Instance;
        if (manager == null) return;

        if (manager.selectedItemSlot == this)
        {
            RemoveHighlight();
            manager.selectedItemSlot = null;
            return;
        }

        if (manager.selectedItemSlot != null)
        {
            manager.selectedItemSlot.RemoveHighlight();
        }

        manager.selectedItemSlot = this;
        AddHighlight();
        manager.ShowConsumeButton();
    }

    public void AddHighlight()
    {
        if (highlightOverlayInstance != null) return;

        GameObject prefab = InventoryManager.Instance.highlightedSlotPrefab;
        if (prefab != null)
        {
            highlightOverlayInstance = Instantiate(prefab, transform);
            highlightOverlayInstance.transform.SetAsFirstSibling();
        }
    }

    public void RemoveHighlight()
    {
        if (highlightOverlayInstance != null)
        {
            Destroy(highlightOverlayInstance);
            highlightOverlayInstance = null;
        }
    }
}
