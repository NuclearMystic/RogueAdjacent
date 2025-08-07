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
        SFXManager.Instance.PlaySFX(incomingItem.itemPickedUpSFX);

        if (incomingItem.stackable)
        {
            foreach (var slot in playerInventory)
            {
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

        foreach (var slot in playerInventory)
        {
            if (slot.ReceiveInventoryItem(incomingItem, 1))
            {
                pickedUp = true;
                return;
            }
        }
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