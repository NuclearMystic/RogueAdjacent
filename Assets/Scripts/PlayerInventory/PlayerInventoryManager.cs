using System.Linq;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    public static PlayerInventoryManager Instance { get; private set; }

    private UIManager UIManager;
    public InventoryManager inventoryManager;
    public CharacterManager characterManager;
    public ItemSlot[] playerInventory;
    public bool isInventoryFull = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        UIManager = FindFirstObjectByType<UIManager>();

        inventoryManager = UIManager.InventoryMenu.GetComponentInChildren<InventoryManager>();
        characterManager = UIManager.CharacterMenu.GetComponentInChildren<CharacterManager>();
    }

    private void Update()
    {
        playerInventory = characterManager.characterItemSlots
                          .Concat(inventoryManager.inventoryItemSlots).ToArray();
    }

    public void CheckIfInventoryFull()
    {
        int slotsFilled = 0;

        foreach (var slot in playerInventory)
        {
            if (slot.slotFilled)
                slotsFilled++;
        }

        isInventoryFull = slotsFilled >= playerInventory.Length;
    }

    public void PickUpItem(in InventoryItem incomingItem, out bool pickedUp)
    {
        pickedUp = false;

        if (incomingItem == null)
        {
            Debug.LogWarning("PickUpItem called with null incomingItem.");
            return;
        }

        // Make sure our managers & slot cache exist
        if (playerInventory == null || playerInventory.Length == 0)
        {
            // Try to (re)bind dependencies if needed
            if (UIManager == null) UIManager = FindFirstObjectByType<UIManager>();
            if (inventoryManager == null && UIManager != null)
                inventoryManager = UIManager.InventoryMenu?.GetComponentInChildren<InventoryManager>();
            if (characterManager == null && UIManager != null)
                characterManager = UIManager.CharacterMenu?.GetComponentInChildren<CharacterManager>();

            if (characterManager != null && inventoryManager != null)
            {
                playerInventory = characterManager.characterItemSlots
                                  .Concat(inventoryManager.inventoryItemSlots)
                                  .ToArray();
            }
        }

        if (playerInventory == null || playerInventory.Length == 0)
        {
            Debug.LogWarning("PickUpItem: playerInventory not ready yet.");
            return;
        }

        // SFX is optional — guard instance too
        if (SFXManager.Instance != null)
            SFXManager.Instance.PlaySFX(incomingItem.itemPickedUpSFX);

        // 1) Try stacking into Any-slots
        if (incomingItem.stackable)
        {
            foreach (var slot in playerInventory)
            {
                if (slot == null) continue; // guard!

                if (slot.slotType == ItemSlot.SlotType.Any)
                {
                    if (slot.ReceiveInventoryItem(incomingItem, 1))
                    {
                        pickedUp = true;
                        return;
                    }
                }
            }
        }

        // 2) Try first empty/compatible slot
        foreach (var slot in playerInventory)
        {
            if (slot == null) continue; // guard!

            if (slot.ReceiveInventoryItem(incomingItem, 1))
            {
                pickedUp = true;
                return;
            }
        }

        Debug.LogWarning($"PickUpItem: No space for {incomingItem.ObjectName}");
    }

    public void RemoveItemsById(int itemId, int quantity)
    {
        int remaining = quantity; 

        foreach (var slot in characterManager.characterItemSlots.Concat(inventoryManager.inventoryItemSlots))
        {
            if (slot.inventoryItem != null && slot.inventoryItem.itemId == itemId)
            {
                int removable = Mathf.Min(remaining, slot.heldItems);
                slot.heldItems -= removable;
                remaining -= removable;

                slot.draggableIconSlot.UpdateQuantity(slot.heldItems);

                if (slot.heldItems <= 0)
                {
                    slot.ClearSlot();
                }

                if (remaining <= 0)
                    return;
            }
        }

        Debug.LogWarning($"Tried to remove {quantity} items with ID {itemId}, but only {quantity - remaining} were removed.");
    }
}