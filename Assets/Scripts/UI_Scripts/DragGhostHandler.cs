// DragGhostHandler.cs
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DragGhostHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI quantityText;

    private InventoryItem currentItem;
    private int currentQuantity;
    private ItemSlot originalSlot;

    public void BeginDrag(InventoryItem item, int quantity, ItemSlot originSlot = null)
    {
        currentItem = item;
        currentQuantity = quantity;
        originalSlot = originSlot;

        if (iconImage != null && item != null)
        {
            iconImage.sprite = item.ObjectIcon;
            iconImage.color = Color.white;
        }

        if (quantityText != null)
        {
            quantityText.text = quantity > 1 ? quantity.ToString() : "";
        }

        transform.SetAsLastSibling();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // No logic required here
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        EndDrag();
    }

    public void EndDrag()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        ItemSlot targetSlot = null;
        foreach (var result in results)
        {
            targetSlot = result.gameObject.GetComponentInParent<ItemSlot>();
            if (targetSlot != null) break;
        }

        int remainingAmount = currentQuantity;

        if (targetSlot != null && targetSlot.CanAcceptItem(currentItem))
        {
            // If slot already has the same item and room, merge instead of full receive
            if (targetSlot.inventoryItem != null &&
                targetSlot.inventoryItem.itemId == currentItem.itemId &&
                !targetSlot.slotFilled)
            {
                int spaceLeft = targetSlot.maxHeldItems - targetSlot.heldItems;
                int amountToMerge = Mathf.Min(spaceLeft, remainingAmount);

                targetSlot.draggableIconSlot.SetQuantity(
                    targetSlot.draggableIconSlot.quantity + amountToMerge);
                targetSlot.heldItems += amountToMerge;
                targetSlot.slotFilled = targetSlot.heldItems >= targetSlot.maxHeldItems;

                remainingAmount -= amountToMerge;
            }
            else
            {
                // Slot was empty or a different item; try full receive
                bool success = targetSlot.ReceiveInventoryItem(currentItem, remainingAmount);
                if (success)
                {
                    remainingAmount = 0;
                }
            }
        }

        // Return leftover to original slot if any remains
        if (remainingAmount > 0)
        {
            if (originalSlot != null)
            {
                originalSlot.ReceiveInventoryItem(currentItem, remainingAmount);
            }
            else
            {
                DropItemToWorld();
            }
        }

        ResetGhost();
    }


    private void DropItemToWorld()
    {
        if (currentItem == null || currentItem.itemPrefab == null) return;

        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null) return;

        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Vector3 dragDirection = (Input.mousePosition - screenCenter).normalized;
        Vector3 worldDirection = Camera.main.transform.TransformDirection(dragDirection);
        worldDirection.z = 0;
        worldDirection.Normalize();

        float baseDistance = .1f;

        for (int i = 0; i < currentQuantity; i++)
        {
            float offset = 0.1f * i;
            Vector3 dropPosition = player.position + worldDirection * (baseDistance + offset);

            RaycastHit2D hit = Physics2D.Raycast(dropPosition, Vector2.down, 5f);
            if (hit.collider != null)
            {
                dropPosition = hit.point;
            }

            Instantiate(currentItem.itemPrefab, dropPosition, Quaternion.identity);
        }
    }

    private void ResetGhost()
    {
        currentItem = null;
        currentQuantity = 0;
        originalSlot = null;

        if (iconImage != null)
        {
            iconImage.sprite = null;
            iconImage.color = new Color(1, 1, 1, 0);
        }

        if (quantityText != null)
        {
            quantityText.text = "";
        }

        gameObject.SetActive(false);
    }
}
