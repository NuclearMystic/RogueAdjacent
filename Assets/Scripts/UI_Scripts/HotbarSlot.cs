using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class HotbarSlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    [Header("References")]
    public Image iconImage;
    public TextMeshProUGUI quantityText;

    [Header("Runtime")]
    public ItemSlot originSlot;

    private void Update()
    {
        if (originSlot != null)
        {
            if (originSlot.HasItem())
            {
                iconImage.sprite = originSlot.inventoryItem.ObjectIcon;
                iconImage.color = Color.white;
                UpdateQuantity();
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        DraggableIconSlot draggedIcon = eventData.pointerDrag?.GetComponent<DraggableIconSlot>();
        if (draggedIcon == null || draggedIcon.slotItem == null) return;

        ItemSlot sourceSlot = draggedIcon.GetComponentInParent<ItemSlot>();
        if (sourceSlot == null || !sourceSlot.HasItem()) return;

        if (sourceSlot.slotType != ItemSlot.SlotType.Any) return;

        HotbarManager.Instance.AssignItemToHotbar(sourceSlot);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (originSlot != null)
            {
                Debug.Log($"Unassigned hotbar slot from item: {originSlot.inventoryItem.ObjectName}");
                ClearSlot();
            }
        }
    }
    public void AssignReference(ItemSlot origin)
    {
        originSlot = origin;

        // Only update visuals if origin has an item
        if (origin != null && origin.HasItem() && origin.inventoryItem != null)
        {
            iconImage.sprite = origin.inventoryItem.ObjectIcon;
            iconImage.color = Color.white;
            UpdateQuantity();
        }
        else
        {
            iconImage.sprite = null;
            iconImage.color = new Color(1, 1, 1, 0);
            quantityText.text = "";
        }
    }

    public void UpdateQuantity()
    {
        if (originSlot != null && originSlot.heldItems > 1)
        {
            quantityText.text = originSlot.heldItems.ToString();
        }
        else
        {
            quantityText.text = "";
        }
    }

    public void ClearSlot()
    {
        if (originSlot != null && originSlot.inventoryItem is EquipmentItem equipItem)
        {
            int equippedIndex = PlayerEquipmentManager.Instance.equippedWeapons
                                 .ToList()
                                 .FindIndex(e => e == equipItem);

            if (equippedIndex >= 0)
            {
                PlayerEquipmentManager.Instance.UnequipWeaponItem(equippedIndex);
                if (PlayerEquipmentManager.Instance.currentHeldWeapon == equippedIndex + 1)
                    PlayerEquipmentManager.Instance.SetCurrentHeldWeapon(0);
            }
        }

        originSlot = null;
        iconImage.sprite = null;
        iconImage.color = new Color(1, 1, 1, 0);
        quantityText.text = "";
    }

    public bool HasItem()
    {
        return originSlot != null && originSlot.HasItem();
    }

    public InventoryItem GetItem()
    {
        return originSlot != null ? originSlot.inventoryItem : null;
    }
}
