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

    private Canvas mainUICanvas;
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
            GameObject crosshairGO = GameObject.FindGameObjectWithTag("Crosshair");
            if (crosshairGO != null)
                crosshair = crosshairGO.GetComponent<RectTransform>();
        }

        if (mainUICanvas == null && UIManager.Instance != null)
        {
            mainUICanvas = UIManager.Instance.GetComponentInParent<Canvas>();
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

        if (mainUICanvas != null)
        {
            transform.SetParent(mainUICanvas.transform);
        }
        else
        {
            transform.SetParent(transform.root);
        }

        transform.SetAsLastSibling();
        iconImage.raycastTarget = false;

        if (parentAfterDrag == ShopManager.Instance.sellSlotParent)
        {
            ShopManager.Instance.UnregisterSellItem(this);
        }

        gameObject.SetActive(true);

        if (InventoryManager.Instance.selectedItemSlot == this)
        {
            InventoryManager.Instance.selectedItemSlot = null;
            RemoveHighlight();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (crosshair != null)
        {
            transform.position = crosshair.position;
        }
        else
        {
            transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        iconImage.raycastTarget = true;

        GameObject hovered = eventData.pointerEnter;

        if (hovered == gameObject || (hovered != null && hovered.transform.IsChildOf(transform)))
        {
            hovered = null;
        }

        bool droppedInUI = false;

        if (hovered != null)
        {
            if (hovered.transform.IsChildOf(ShopManager.Instance.sellSlotParent))
            {
                transform.SetParent(ShopManager.Instance.sellSlotParent);
                ShopManager.Instance.RegisterSellItem(this);
                droppedInUI = true;
            }
            else if (hovered.TryGetComponent<ItemSlot>(out var targetSlot))
            {
                if (targetSlot.CanAcceptItem(slotItem))
                {
                    transform.SetParent(targetSlot.transform);
                    droppedInUI = true;
                }
                else
                {
                    transform.SetParent(parentAfterDrag);
                    transform.localPosition = Vector3.zero;
                    return;
                }
            }
            else if (hovered.transform.IsChildOf(parentAfterDrag))
            {
                transform.SetParent(parentAfterDrag);
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
            Debug.LogWarning("No item or world prefab to drop.");
            Destroy(gameObject);
            return;
        }

        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found in scene!");
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

        Debug.Log($"Dropped {quantity}x {slotItem.ObjectName} into world");

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
        {
            Debug.LogWarning("InventoryManager.Instance is null.");
            return;
        }

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
