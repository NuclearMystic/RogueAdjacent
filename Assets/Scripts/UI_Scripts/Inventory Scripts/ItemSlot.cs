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

        if (inventoryItem != null && inventoryItem.itemId == draggedIcon.slotItem.itemId && heldItems < maxHeldItems)
        {
            int availableSpace = maxHeldItems - heldItems;
            int added = Mathf.Min(transferAmount, availableSpace);

            heldItems += added;
            draggableIconSlot.UpdateQuantity(heldItems);
            slotFilled = heldItems >= maxHeldItems;

            int leftover = transferAmount - added;
            if (leftover > 0)
                draggedIcon.SetQuantity(leftover);
            else
                Destroy(draggedIcon.gameObject);

            return;
        }

        if (inventoryItem == null)
        {
            draggedIcon.parentAfterDrag = transform;
            return;
        }

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

    private void Update() => UpdateInventoryItem();

    private void UpdateInventoryItem()
    {
        if (transform.childCount > 0)
        {
            draggableIconSlot = GetComponentInChildren<DraggableIconSlot>();
            if (draggableIconSlot.slotItem == null) return;

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
    }

    public bool ReceiveInventoryItem(InventoryItem incomingItem, int amount)
    {
        if (incomingItem == null) return false;

        if (inventoryItem == null)
        {
            var iconGO = Instantiate(incomingItem.draggableIcon, transform);
            var icon = iconGO.GetComponent<DraggableIconSlot>();
            icon.slotItem = incomingItem;
            icon.SetQuantity(amount);

            inventoryItem = incomingItem;
            draggableIconSlot = icon;
            heldItems = amount;
            maxHeldItems = incomingItem.stackSize;
            slotFilled = heldItems >= maxHeldItems;

            return true;
        }

        if (inventoryItem.itemId != incomingItem.itemId) return false;
        if (slotFilled) return false;

        int spaceLeft = maxHeldItems - heldItems;
        int amountToAdd = Mathf.Min(spaceLeft, amount);

        draggableIconSlot.SetQuantity(draggableIconSlot.quantity + amountToAdd);
        heldItems += amountToAdd;
        slotFilled = heldItems >= maxHeldItems;

        return amountToAdd == amount;
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

            HotbarManager.Instance?.UnassignHotbarSlotByOrigin(this);
        }
    }

    public bool HasItem() => inventoryItem != null;
}