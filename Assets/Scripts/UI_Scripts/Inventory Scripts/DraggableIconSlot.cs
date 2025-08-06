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
    public GameObject crosshairGO;

    private GameObject highlightOverlayInstance;

    private void Awake()
    {
        if (quantityLabel == null)
        {
            var labelTransform = transform.Find("QuantityLabel");
            if (labelTransform != null)
                quantityLabel = labelTransform.GetComponent<TextMeshProUGUI>();
        }

        if (slotItem != null)
        {
            iconImage.sprite = slotItem.ObjectIcon;
        }
    }

    private void Start()
    {
        if (crosshair == null)
        {
            crosshairGO = GameObject.FindGameObjectWithTag("Crosshair");
            if (crosshairGO != null)
                crosshair = crosshairGO.GetComponent<RectTransform>();
        }
    }

    private void Update()
    {
        if (slotItem == null)
        {
            iconImage.color = new Color(0, 0, 0, 0);
            UpdateQuantity(0);
        }
        else
        {
            iconImage.color = Color.white;
            iconImage.sprite = slotItem.ObjectIcon;
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
        parentAfterDrag = transform.parent;

        var invManager = InventoryManager.Instance;
        int dragQty = 1;

        if (invManager != null && invManager.selectedItemSlot == this &&
            invManager.qtySlider != null && invManager.qtySlider.gameObject.activeInHierarchy)
        {
            dragQty = Mathf.Clamp(Mathf.RoundToInt(invManager.qtySlider.value), 1, quantity);
        }

        if (dragQty >= quantity)
        {
            dragQty = quantity;

            transform.SetParent(crosshairGO.transform, false);
            transform.SetAsLastSibling();
            iconImage.raycastTarget = false;

            if (parentAfterDrag == ShopManager.Instance.sellSlotParent)
            {
                ShopManager.Instance.UnregisterSellItem(this);
            }

            if (invManager.selectedItemSlot == this)
            {
                invManager.selectedItemSlot = null;
                RemoveHighlight();
            }

            return;
        }

        quantity -= dragQty;
        UpdateQuantity(quantity);

        GameObject dragGO = Instantiate(gameObject);
        DraggableIconSlot newIcon = dragGO.GetComponent<DraggableIconSlot>();

        newIcon.slotItem = this.slotItem;
        newIcon.SetQuantity(dragQty);
        newIcon.parentAfterDrag = parentAfterDrag;

        if (crosshairGO != null)
        {
            dragGO.transform.SetParent(crosshairGO.transform, false);
            dragGO.transform.SetAsLastSibling();
        }

        newIcon.iconImage.raycastTarget = false;

        if (parentAfterDrag == ShopManager.Instance.sellSlotParent)
        {
            ShopManager.Instance.UnregisterSellItem(this);
        }

        transform.SetAsLastSibling();

        if (invManager.selectedItemSlot == this)
        {
            invManager.selectedItemSlot = null;
            RemoveHighlight();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //if (crosshair != null)
        //    transform.position = crosshair.position;
        //else
        //    transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        iconImage.raycastTarget = true;

        bool droppedInUI = false;
        GameObject hovered = eventData.pointerEnter;

        if (hovered != null && hovered != gameObject)
        {
            if (hovered.transform.TryGetComponent<ItemSlot>(out var targetSlot))
            {
                if (targetSlot.CanAcceptItem(slotItem))
                {
                    transform.SetParent(targetSlot.transform);
                    transform.localPosition = Vector3.zero;

                    targetSlot.draggableIconSlot = this;
                    targetSlot.inventoryItem = slotItem;
                    targetSlot.heldItems = quantity;
                    targetSlot.maxHeldItems = slotItem.stackSize;
                    targetSlot.slotFilled = quantity >= slotItem.stackSize;

                    droppedInUI = true;
                }
            }
            else if (hovered.transform.IsChildOf(parentAfterDrag))
            {
                transform.SetParent(parentAfterDrag);
                transform.localPosition = Vector3.zero;
                droppedInUI = true;
            }
        }

        if (!droppedInUI && !EventSystem.current.IsPointerOverGameObject())
        {
            DropItemToWorld();
            return;
        }

        transform.localPosition = Vector3.zero;
    }

    private void DropItemToWorld()
    {
        if (slotItem == null || slotItem.itemPrefab == null)
        {
            Destroy(gameObject);
            return;
        }

        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Vector3 dragDirection = (Input.mousePosition - screenCenter).normalized;
        Vector3 worldDirection = Camera.main.transform.TransformDirection(dragDirection);
        worldDirection.z = 0;
        worldDirection.Normalize();

        float baseDistance = .1f;

        for (int i = 0; i < quantity; i++)
        {
            float offset = 0.1f * i;
            Vector3 dropPosition = player.position + worldDirection * (baseDistance + offset);

            RaycastHit2D hit = Physics2D.Raycast(dropPosition, Vector2.down, 5f);
            if (hit.collider != null)
            {
                dropPosition = hit.point;
            }

            Instantiate(slotItem.itemPrefab, dropPosition, Quaternion.identity);
        }

        if (parentAfterDrag.TryGetComponent<ItemSlot>(out var slot))
        {
            slot.heldItems -= quantity;

            if (slot.draggableIconSlot != null)
                slot.draggableIconSlot.UpdateQuantity(slot.heldItems);

            if (slot.heldItems <= 0)
                slot.ClearSlot();
        }

        Destroy(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var manager = InventoryManager.Instance;

        if (manager == null)
            return;

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
