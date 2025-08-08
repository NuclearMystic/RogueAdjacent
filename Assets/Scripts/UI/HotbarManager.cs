using System.Linq;
using UnityEngine;

public class HotbarManager : MonoBehaviour
{
    public static HotbarManager Instance { get; private set; }

    [Header("Hotbar Slot References")]
    public HotbarSlot[] hotbarSlots;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                UseSlot(i);
            }
        }
    }

    public void AssignToFirstEmptyHotbar(ItemSlot originSlot)
    {
        if (originSlot == null || !originSlot.HasItem())
            return;

        foreach (var hotbar in hotbarSlots)
        {
            if (hotbar.originSlot == originSlot)
            {
                return;
            }
        }

        foreach (var hotbar in hotbarSlots)
        {
            if (!hotbar.HasItem())
            {
                hotbar.AssignReference(originSlot);
                return;
            }
        }
    }

    public void UseSlot(int index)
    {
        if (index < 0 || index >= hotbarSlots.Length) return;


        HotbarSlot slot = hotbarSlots[index];
        if (slot == null || slot.originSlot == null)
        {
            return;
        }

        InventoryItem item = slot.originSlot.inventoryItem;
        if (item == null)
        {
            return;
        }

        bool isConsumable = item.healthEffect > 0 || item.staminaEffect > 0 || item.magicEffect > 0;

        if (isConsumable)
        {
            PlayerVitals.instance?.RestoreHealth(item.healthEffect);
            PlayerVitals.instance?.RestoreStamina(item.staminaEffect);
            PlayerVitals.instance?.ReplenishMagic(item.magicEffect);

            SFXManager.Instance?.PlaySFX(item.itemUsedSFX);

            slot.originSlot.heldItems--;
            slot.originSlot.draggableIconSlot?.UpdateQuantity(slot.originSlot.heldItems);

            if (slot.originSlot.heldItems <= 0)
            {
                slot.originSlot.ClearSlot();
                slot.ClearSlot();
            }

            return;
        }

        if (item is EquipmentItem equipItem)
        {
            int equippedIndex = PlayerEquipmentManager.Instance.equippedWeapons
                                 .ToList()
                                 .FindIndex(e => e == equipItem);

            if (equippedIndex >= 0)
            {
                if (PlayerEquipmentManager.Instance.currentHeldWeapon == equippedIndex + 1)
                {
                    PlayerEquipmentManager.Instance.SetCurrentHeldWeapon(0);
                }
                else
                {
                    PlayerEquipmentManager.Instance.SetCurrentHeldWeapon(equippedIndex + 1);
                }
            }
            else
            {
            }
        }
    }

    public void RefreshAllHotbarSlots()
    {
        foreach (var slot in hotbarSlots)
        {
            slot.UpdateQuantity();
        }
    }
    public void AssignItemToHotbar(ItemSlot originSlot)
    {
        if (originSlot == null || !originSlot.HasItem()) return;

        foreach (var slot in hotbarSlots)
        {
            if (slot.originSlot == originSlot)
            {
                return;
            }
        }

        foreach (var slot in hotbarSlots)
        {
            if (slot.originSlot == null)
            {
                slot.AssignReference(originSlot);
                return;
            }
        }
    }

    public void UnassignHotbarSlotByOrigin(ItemSlot originSlot)
    {
        foreach (var hotbarSlot in hotbarSlots)
        {
            if (hotbarSlot.originSlot == originSlot)
            {
                hotbarSlot.ClearSlot();
                break;
            }
        }
    }

    public bool IsSlotAssigned(ItemSlot originSlot)
    {
        return hotbarSlots.Any(slot => slot.originSlot == originSlot);
    }


}
