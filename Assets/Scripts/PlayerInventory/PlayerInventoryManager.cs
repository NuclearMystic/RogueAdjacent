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
        UIManager = UIManager.Instance;

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

        // Try stacking
        if (incomingItem.stackable)
        {
            foreach (var slot in playerInventory)
            {
                if (slot.ReceiveInventoryItem(incomingItem))
                {
                    pickedUp = true;
                    return;
                }
            }
        }

        // Try empty slot
        foreach (var slot in playerInventory)
        {
            if (slot.ReceiveInventoryItem(incomingItem))
            {
                pickedUp = true;
                return;
            }
        }
    }
}
