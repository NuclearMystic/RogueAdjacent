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
            if ((hotbarSlots[i].originSlot == null || hotbarSlots[i].originSlot.inventoryItem == null) && hotbarSlots[i].iconImage != null) hotbarSlots[i].ClearSlot();
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                UseSlot(i);
            }
        }
    }
    public bool EnsureWeaponInWeaponSlot(ItemSlot originSlot, EquipmentItem equipItem, out ItemSlot weaponSlotOut)
    {
        weaponSlotOut = null;

        var pim = PlayerInventoryManager.Instance;
        var charMgr = pim != null ? pim.characterManager : null;
        if (charMgr == null || equipItem == null || originSlot == null) return false;

        for (int i = 0; i < charMgr.characterWeaponSlots.Length; i++)
        {
            if (charMgr.characterWeaponSlots[i] == originSlot)
            {
                weaponSlotOut = originSlot;
                return true;
            }
        }

        ItemSlot emptyWeaponSlot = null;
        for (int i = 0; i < charMgr.characterWeaponSlots.Length; i++)
        {
            var ws = charMgr.characterWeaponSlots[i];
            if (ws != null && ws.inventoryItem == null)
            {
                emptyWeaponSlot = ws;
                break;
            }
        }
        if (emptyWeaponSlot == null) return false;

        if (emptyWeaponSlot.ReceiveInventoryItem(equipItem, 1))
        {
            originSlot.heldItems = Mathf.Max(0, originSlot.heldItems - 1);
            originSlot.draggableIconSlot?.UpdateQuantity(originSlot.heldItems);
            if (originSlot.heldItems <= 0) originSlot.ClearSlot();

            weaponSlotOut = emptyWeaponSlot;
            return true;
        }

        return false;
    }

    public bool TryEquipToWeaponSlotAndBind(ItemSlot originSlot, EquipmentItem equipItem)
    {
        var pim = PlayerInventoryManager.Instance;
        var charMgr = pim != null ? pim.characterManager : null;
        if (charMgr == null || equipItem == null || originSlot == null) return false;

        for (int i = 0; i < charMgr.characterWeaponSlots.Length; i++)
        {
            if (charMgr.characterWeaponSlots[i] == originSlot)
            {
                AssignItemToHotbar(originSlot); 
                return true;
            }
        }

        ItemSlot emptyWeaponSlot = null;
        for (int i = 0; i < charMgr.characterWeaponSlots.Length; i++)
        {
            var ws = charMgr.characterWeaponSlots[i];
            if (ws != null && ws.inventoryItem == null)
            {
                emptyWeaponSlot = ws;
                break;
            }
        }

        if (emptyWeaponSlot == null)
            return false; 

        if (emptyWeaponSlot.ReceiveInventoryItem(equipItem, 1))
        {
            originSlot.heldItems = Mathf.Max(0, originSlot.heldItems - 1);
            originSlot.draggableIconSlot?.UpdateQuantity(originSlot.heldItems);
            if (originSlot.heldItems <= 0) originSlot.ClearSlot();

            AssignItemToHotbar(emptyWeaponSlot);
            return true;
        }

        return false;
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
        if (slot == null || slot.originSlot == null) return;

        var item = slot.originSlot.inventoryItem;
        if (item == null) return;

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

        if (item is EquipmentItem)
        {
            int equippedIndex = slot.weaponSlotIndex;

            if (equippedIndex < 0)
            {
                var pim = PlayerInventoryManager.Instance;
                var charMgr = pim != null ? pim.characterManager : null;
                if (charMgr != null)
                {
                    for (int i = 0; i < charMgr.characterWeaponSlots.Length; i++)
                    {
                        if (charMgr.characterWeaponSlots[i] == slot.originSlot)
                        {
                            equippedIndex = i;
                            slot.weaponSlotIndex = i; 
                            break;
                        }
                    }
                }
            }

            if (equippedIndex >= 0)
            {
                if (PlayerEquipmentManager.Instance.currentHeldWeapon == equippedIndex + 1)
                    PlayerEquipmentManager.Instance.SetCurrentHeldWeapon(0);
                else
                    PlayerEquipmentManager.Instance.SetCurrentHeldWeapon(equippedIndex + 1);
            }

            return;
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
