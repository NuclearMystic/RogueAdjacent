using System.Linq;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    private UIManager UIManager;
    public InventoryManager inventoryManager;
    public CharacterManager characterManager;
    public ItemSlot[] playerInventory;
    public bool isInventoryFull = false;

    private void Start()
    {
        UIManager = FindFirstObjectByType<UIManager>();  

        inventoryManager = UIManager.InventoryMenu.GetComponentInChildren<InventoryManager>();    
        characterManager = UIManager.CharacterMenu.GetComponentInChildren<CharacterManager>();

    }

    private void Update()
    {
        playerInventory = characterManager.characterItemSlots.Concat(inventoryManager.inventoryItemSlots).ToArray();
    }

    public void CheckIfInventoryFull()
    {
        int slotsFilled = 0;
        
        for (int i = 0; i < playerInventory.Length; i++)
        {
            if (playerInventory[i].slotFilled)
            {
                slotsFilled++;
            }
        }
        if (slotsFilled >= playerInventory.Length)
        {
            isInventoryFull = true;
        }
        else
        {
            isInventoryFull = false;
        }
    }

    public void PickUpItem(in InventoryItem incomingItem, out bool pickedUp)
    {
        for (int i = 0; i < playerInventory.Length; i++)
        {
            if (playerInventory[i].slotFilled == false && playerInventory[i].slotType == ItemSlot.SlotType.Any)
            {
                playerInventory[i].ReceiveInventoryItem(incomingItem);
                //playerInventory[i].UpdateInventoryItem();
                pickedUp = true;
                return;
            }
            else if (playerInventory[i].slotFilled == false && playerInventory[i].slotType != ItemSlot.SlotType.Any)
            {
                if (incomingItem.itemType.ToString() == playerInventory[i].slotType.ToString())
                {
                    playerInventory[i].ReceiveInventoryItem(incomingItem);
                   //playerInventory[i].UpdateInventoryItem();
                    pickedUp = true;
                    return;
                }            
            }           
        }
        pickedUp = false;
    }
}
