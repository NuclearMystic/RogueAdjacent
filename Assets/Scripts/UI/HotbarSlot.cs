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
    public int weaponSlotIndex = -1;

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
    private void LateUpdate()
    {
        if (originSlot == null || !originSlot.HasItem())
        {
            ClearSlot();
            return;
        }
        UpdateQuantity();
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

        weaponSlotIndex = -1;
        var pim = PlayerInventoryManager.Instance;
        var charMgr = pim != null ? pim.characterManager : null;
        if (charMgr != null && originSlot != null && originSlot.slotType == ItemSlot.SlotType.Weapon)
        {
            for (int i = 0; i < charMgr.characterWeaponSlots.Length; i++)
            {
                if (charMgr.characterWeaponSlots[i] == originSlot)
                {
                    weaponSlotIndex = i;
                    break;
                }
            }
        }

        if (originSlot != null && originSlot.HasItem())
        {
            iconImage.sprite = originSlot.inventoryItem.ObjectIcon;
            iconImage.color = Color.white;
            UpdateQuantity();
        }
        else
        {
            ClearSlot();
        }
    }

    public void UpdateQuantity()
    {
        if (originSlot != null && originSlot.HasItem() && originSlot.heldItems > 1)
            quantityText.text = originSlot.heldItems.ToString();
        else
            quantityText.text = "";
    }

    public void ClearSlot()
    {
        originSlot = null;
        weaponSlotIndex = -1; // reset cache
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
