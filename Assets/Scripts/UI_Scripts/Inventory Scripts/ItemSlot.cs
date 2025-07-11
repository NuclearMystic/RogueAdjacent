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

    public enum SlotType { Any, Weapon, Hat, FaceAcces, Cape, Outfit }
    public SlotType slotType;
    public bool isItemEquipped = false;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DraggableIconSlot draggedIcon = dropped.GetComponent<DraggableIconSlot>();

        if (draggedIcon == null || draggedIcon.slotItem == null) return;

        if (slotType != SlotType.Any && draggedIcon.slotItem.itemType.ToString() != slotType.ToString()) return;

        if (transform.childCount == 0)
        {
            draggedIcon.parentAfterDrag = transform;
        }

        if (slotType != SlotType.Any && draggedIcon.slotItem is EquipmentItem equipItem)
        {
            int index = transform.GetSiblingIndex();
            if (IsWeaponSlot())
            {
                PlayerEquipmentManager.Instance.EquipWeaponItem(index, equipItem);
            }
            else
            {
                PlayerEquipmentManager.Instance.EquipArmorItem(index, equipItem);
            }
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
            inventoryItem = GetComponentInChildren<DraggableIconSlot>().slotItem;
            draggableIconSlot = GetComponentInChildren<DraggableIconSlot>();
            maxHeldItems = inventoryItem.stackSize;
            heldItems = transform.childCount;
            slotFilled = heldItems == maxHeldItems;
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
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            bool shouldShow = (i == 0);
            foreach (var image in child.GetComponentsInChildren<Image>())
                image.enabled = shouldShow;
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
            Instantiate(receivedItem.draggableIcon, transform);
            draggableIconSlot = GetComponentInChildren<DraggableIconSlot>();
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
