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

    public enum SlotType { Any, Weapon, Hat, FaceAcces, Cape, Outfit }
    public SlotType slotType;
    public bool isItemEquipped = false;

    [Header("Loot Box Slot?")]
    public bool isLootSlot = false;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DraggableIconSlot draggedIcon = dropped?.GetComponent<DraggableIconSlot>();
        if (draggedIcon == null || draggedIcon.slotItem == null) return;

        if (slotType != SlotType.Any && draggedIcon.slotItem.itemType.ToString() != slotType.ToString())
            return;

        int transferAmount = draggedIcon.quantity;

        // Merge stack
        if (inventoryItem != null &&
            inventoryItem.itemId == draggedIcon.slotItem.itemId &&
            heldItems < maxHeldItems)
        {
            int availableSpace = maxHeldItems - heldItems;
            int added = Mathf.Min(transferAmount, availableSpace);

            heldItems += added;
            draggableIconSlot.UpdateQuantity(heldItems);
            slotFilled = heldItems >= maxHeldItems;

            // Return remainder back to dragged icon
            int leftover = transferAmount - added;
            if (leftover > 0)
            {
                draggedIcon.SetQuantity(leftover);
            }
            else
            {
                Destroy(draggedIcon.gameObject);
            }

            return;
        }

        // Empty slot - accept full stack
        if (inventoryItem == null)
        {
            draggedIcon.parentAfterDrag = transform;
            return;
        }

        // Equip if relevant
        if (slotType != SlotType.Any && draggedIcon.slotItem is EquipmentItem equipItem)
        {
            int index = transform.GetSiblingIndex();
            if (IsWeaponSlot())
                PlayerEquipmentManager.Instance.EquipWeaponItem(index, equipItem);
            else
                PlayerEquipmentManager.Instance.EquipArmorItem(index, equipItem);
        }
    }


    private bool IsWeaponSlot() => slotType == SlotType.Weapon;

    private void Update()
    {
        UpdateInventoryItem();
    }

    private void UpdateInventoryItem()
    {
        if (transform.childCount > 0)
        {
            draggableIconSlot = GetComponentInChildren<DraggableIconSlot>();

            if (draggableIconSlot.slotItem == null)
            {
                return;
            }
            inventoryItem = draggableIconSlot.slotItem;

            heldItems = draggableIconSlot.quantity;
            maxHeldItems = inventoryItem.stackSize;
            slotFilled = heldItems >= maxHeldItems;
        }
        else
        {
            heldItems = 0;
            maxHeldItems = 0;
            inventoryItem = null;
            draggableIconSlot = null;
            slotFilled = false;
        }

        UpdateStackVisuals();
    }

    private void UpdateStackVisuals()
    {
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    GameObject child = transform.GetChild(i).gameObject;
        //    bool shouldShow = (i == 0);

        //    foreach (var image in child.GetComponentsInChildren<Image>())
        //        image.enabled = shouldShow;

        //    foreach (var canvasGroup in child.GetComponentsInChildren<CanvasGroup>())
        //    {
        //        canvasGroup.alpha = shouldShow ? 1f : 0f;
        //        canvasGroup.blocksRaycasts = shouldShow;
        //    }
        //}
    }

    public bool ReceiveInventoryItem(InventoryItem receivedItem, int quantityToAdd)
    {
        if (inventoryItem != null &&
            inventoryItem.itemId == receivedItem.itemId &&
            heldItems < maxHeldItems)
        {
            int availableSpace = maxHeldItems - heldItems;
            int toAdd = Mathf.Min(quantityToAdd, availableSpace);

            heldItems += toAdd;
            draggableIconSlot.UpdateQuantity(heldItems);
            slotFilled = heldItems >= maxHeldItems;
            return true;
        }

        if (inventoryItem == null &&
            (slotType == SlotType.Any || receivedItem.itemType.ToString() == slotType.ToString()))
        {
            GameObject iconGO = Instantiate(receivedItem.draggableIcon, transform);
            draggableIconSlot = iconGO.GetComponent<DraggableIconSlot>();
            draggableIconSlot.slotItem = receivedItem;
            draggableIconSlot.UpdateQuantity(quantityToAdd);

            inventoryItem = receivedItem;
            heldItems = quantityToAdd;
            maxHeldItems = receivedItem.stackSize;
            slotFilled = heldItems >= maxHeldItems;

            return true;
        }

        return false;
    }



    public bool CanAcceptItem(InventoryItem item)
    {
        return slotType == SlotType.Any || item.itemType.ToString() == slotType.ToString();
    }

    public void ClearSlot()
    {
        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
            slotFilled = false;
            inventoryItem = null;
            isItemEquipped = false;
        }
    }

    public bool HasItem()
    {
        return inventoryItem != null;
    }
}