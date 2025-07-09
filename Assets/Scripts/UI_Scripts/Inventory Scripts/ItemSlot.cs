using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public Sprite defaultSprite;
    public DraggableIconSlot draggableIconSlot;
    public InventoryItem inventoryItem;
    public bool slotFilled = false;
    public int heldItems;
    public int maxHeldItems;
    public int curItemID;

    public enum SlotType
    {
        Any, 
        Weapon,
        Hat,
        FaceAcces,
        Cape,
        Outfit
    }

    public SlotType slotType;

    public bool isItemEquipped = false;


    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DraggableIconSlot draggedIcon = dropped.GetComponent<DraggableIconSlot>();

        if (draggedIcon == null || draggedIcon.slotItem == null)
            return;

        // Check if the slot accepts the item type
        if (slotType != SlotType.Any && draggedIcon.slotItem.itemType.ToString() != slotType.ToString())
            return;

        // If no children, just parent the dragged item
        if (transform.childCount == 0)
        {
            draggedIcon.parentAfterDrag = transform;
        }
        else
        {
            // Get the first child (assuming one item type per stack)
            DraggableIconSlot existingIcon = transform.GetChild(0).GetComponent<DraggableIconSlot>();

            if (existingIcon != null && existingIcon.slotItem != null)
            {
                // Check if the items are the same and stackable
                if (existingIcon.slotItem.itemId == draggedIcon.slotItem.itemId && existingIcon.slotItem.stackable)
                {
                    // Stack the item by reparenting it under the same slot
                    draggedIcon.parentAfterDrag = transform;

                    // Optional: Update a stack UI count here (e.g., text field)
                }
                else
                {
                    // Optional: Reject or swap if item is different
                    return;
                }
            }
        }
    }
    public void Update()
    {
        //if (inventoryItem != null && transform.childCount == 0 && !slotFilled)
        //{           
        //    slotFilled = true;
        //    Instantiate (inventoryItem.draggableIcon, this.transform);
        //    inventoryItem = null;
        //}
        UpdateInventoryItem();
    }

    private void UpdateInventoryItem()
    {
        if (transform.childCount > 0)
        {
            inventoryItem = GetComponentInChildren<DraggableIconSlot>().slotItem;
            draggableIconSlot = this.GetComponentInChildren<DraggableIconSlot>();
            maxHeldItems = inventoryItem.stackSize;
            heldItems = transform.childCount;

            if (heldItems == maxHeldItems)
            {
                slotFilled = true;
            }
        }
        else if (transform.childCount == 0)
        {
            heldItems = 0;
            maxHeldItems = 0;
            inventoryItem = null;
            draggableIconSlot=null;
            slotFilled = false;
        }
        UpdateStackVisuals();
    }

    private void UpdateStackVisuals()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;

            // Show the first child, hide the rest
            bool shouldShow = (i == 0);
            foreach (var image in child.GetComponentsInChildren<Image>())
            {
                image.enabled = shouldShow;
            }

            foreach (var canvasGroup in child.GetComponentsInChildren<CanvasGroup>())
            {
                canvasGroup.alpha = shouldShow ? 1f : 0f;
                canvasGroup.blocksRaycasts = shouldShow;
            }
        }
    }

    public void ReceiveInventoryItem(InventoryItem receivedItem)
    {
        if (!slotFilled)
        {                 
            Instantiate(receivedItem.draggableIcon, this.transform);
            draggableIconSlot = this.GetComponentInChildren<DraggableIconSlot>();
            inventoryItem = null;
            heldItems++;
        }
    }

    public void ClearSlot()
    {
        if (transform.childCount > 0)
        {
            slotFilled = false;
            inventoryItem = null;
            isItemEquipped = false;
            Destroy(transform.GetChild(0).gameObject);
        }
    }
}
