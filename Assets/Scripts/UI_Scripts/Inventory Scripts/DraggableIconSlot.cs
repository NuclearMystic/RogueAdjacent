using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DraggableIconSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image iconImage;
    public TextMeshProUGUI quantityLabel;
    public InventoryItem slotItem;
    public int quantity = 1;
    public Transform parentAfterDrag;

    private void Awake()
    {
        // Auto-bind quantityLabel if not assigned
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
        transform.SetParent(transform.root); // So it floats on top
        transform.SetAsLastSibling();
        iconImage.raycastTarget = false;

        if (parentAfterDrag == ShopManager.Instance.sellSlotParent)
        {
            ShopManager.Instance.UnregisterSellItem(this);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        iconImage.raycastTarget = true;

        GameObject hovered = eventData.pointerEnter;

        if (hovered != null && hovered.transform.IsChildOf(ShopManager.Instance.sellSlotParent))
        {
            transform.SetParent(ShopManager.Instance.sellSlotParent);
            ShopManager.Instance.RegisterSellItem(this);
        }
        else if (hovered != null && hovered.transform.IsChildOf(parentAfterDrag))
        {
            transform.SetParent(parentAfterDrag);
        }
        else
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
            return;
        }

        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found in scene!");
            return;
        }

        // Determine drop direction from drag
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Vector3 dragDirection = (Input.mousePosition - screenCenter).normalized;

        Vector3 worldDirection = Camera.main.transform.TransformDirection(dragDirection);
        worldDirection.z = 0;
        worldDirection.Normalize();

        float baseDistance = 1f;

        for (int i = 0; i < quantity; i++)
        {
            // Offset each item slightly to avoid perfect overlap
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

        // Remove from original inventory slot
        if (parentAfterDrag.TryGetComponent<ItemSlot>(out var slot))
        {
            slot.heldItems -= quantity;
            slot.draggableIconSlot.UpdateQuantity(slot.heldItems);

            if (slot.heldItems <= 0)
            {
                slot.ClearSlot();
            }
        }

        Destroy(gameObject);
    }

}
