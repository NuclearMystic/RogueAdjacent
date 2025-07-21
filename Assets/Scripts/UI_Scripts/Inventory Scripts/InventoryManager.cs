using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set;  }    

    public ItemSlot[] inventoryItemSlots;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject thisMenu;
    [SerializeField] private TextMeshProUGUI currencyText;

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
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() => CloseWindow());
    }

    private void Update()
    {
        UpdateCurrency();
    }

    public void CloseWindow()
    {
        thisMenu.SetActive(false);
    }

    public void UpdateCurrency()
    {
        StartCoroutine(IUpdateCurrency());
    }

    private IEnumerator IUpdateCurrency()
    {
        yield return null;
        yield return null;
        yield return null;

        currencyText.text = PlayerCurrencyManager.Instance.GetCurrency().ToString();
    }

    public void AddItemToInventory(InventoryItem itemToAdd)
    {
        foreach (var slot in inventoryItemSlots)
        {
            if (slot.inventoryItem != null &&
                slot.inventoryItem.itemId == itemToAdd.itemId &&
                !slot.slotFilled)
            {
                var icon = slot.GetComponentInChildren<DraggableIconSlot>();
                icon.UpdateQuantity(icon.quantity + 1);
                slot.heldItems++;

                slot.slotFilled = slot.heldItems >= slot.maxHeldItems;
                return;
            }
        }

        foreach (var slot in inventoryItemSlots)
        {
            if (slot.inventoryItem == null)
            {
                var iconGO = Instantiate(itemToAdd.draggableIcon, slot.transform);
                var icon = iconGO.GetComponent<DraggableIconSlot>();
                icon.slotItem = itemToAdd;
                icon.UpdateQuantity(1);

                slot.inventoryItem = itemToAdd;
                slot.draggableIconSlot = icon;
                slot.heldItems = 1;
                slot.maxHeldItems = itemToAdd.stackSize;
                return;
            }
        }

        Debug.LogWarning("No available inventory slot!");
    }

    public void AddItemToInventoryWithQuantity(InventoryItem itemToAdd, int amount)
    {
        foreach (var slot in inventoryItemSlots)
        {
            if (slot.inventoryItem != null &&
                slot.inventoryItem.itemId == itemToAdd.itemId &&
                !slot.slotFilled)
            {
                var icon = slot.GetComponentInChildren<DraggableIconSlot>();
                int spaceLeft = slot.maxHeldItems - slot.heldItems;

                int addAmount = Mathf.Min(spaceLeft, amount);
                icon.UpdateQuantity(icon.quantity + addAmount);
                slot.heldItems += addAmount;
                slot.slotFilled = slot.heldItems >= slot.maxHeldItems;

                amount -= addAmount;
                if (amount <= 0) return;
            }
        }

        foreach (var slot in inventoryItemSlots)
        {
            if (slot.inventoryItem == null)
            {
                var iconGO = Instantiate(itemToAdd.draggableIcon, slot.transform);
                var icon = iconGO.GetComponent<DraggableIconSlot>();
                icon.slotItem = itemToAdd;
                int amountToAdd = Mathf.Min(amount, itemToAdd.stackSize);
                icon.SetQuantity(amountToAdd);

                slot.inventoryItem = itemToAdd;
                slot.draggableIconSlot = icon;
                slot.heldItems = amountToAdd;
                slot.maxHeldItems = itemToAdd.stackSize;
                slot.slotFilled = slot.heldItems >= slot.maxHeldItems;

                amount -= amountToAdd;
                if (amount <= 0) return;
            }
        }

        if (amount > 0)
        {
            Debug.LogWarning($"Not enough space for all {itemToAdd.ObjectName} items. {amount} left over.");
        }
    }
}
