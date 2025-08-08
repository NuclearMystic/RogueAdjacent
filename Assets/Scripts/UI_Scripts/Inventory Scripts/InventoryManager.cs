using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("UI Components")]
    public ItemSlot[] inventoryItemSlots;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button consumeButton;
    [SerializeField] private GameObject thisMenu;
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] public Slider qtySlider;
    [SerializeField] private TextMeshProUGUI qtySliderLabel;

    [Header("Prefabs")]
    public GameObject highlightedSlotPrefab;
    public DraggableIconSlot selectedItemSlot;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            Debug.Log("Second Inventory manager destroyed.");
            return;
        }

        Instance = this;
    }
    private void Start()
    {
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() => CloseWindow());
        consumeButton.onClick.RemoveAllListeners();
        consumeButton.onClick.AddListener(() => ConsumeItem());

        qtySlider.onValueChanged.AddListener((val) => {
            qtySliderLabel.text = $"{val}";
        });
    }

    private void Update()
    {
        UpdateCurrency();
        //ShowConsumeButton();
        if (qtySlider.isActiveAndEnabled)
        {
            qtySlider.wholeNumbers = true;
            if (selectedItemSlot != null)
            {
                qtySlider.maxValue = selectedItemSlot.quantity;
            }
        }
        if (selectedItemSlot == null)
        {
            consumeButton.gameObject.SetActive(false);
            qtySlider.gameObject.SetActive(false);
        }
    }

    public void CloseWindow()
    {
        thisMenu.SetActive(false);
    }

    public void ShowConsumeButton()
    {
        if (selectedItemSlot != null && selectedItemSlot.slotItem != null)
        {
            var item = selectedItemSlot.slotItem;
            bool canUse = item.healthEffect > 0 || item.staminaEffect > 0 || item.magicEffect > 0;
            consumeButton.gameObject.SetActive(canUse);

            if (selectedItemSlot.quantity > 1)
            {
                qtySlider.gameObject.SetActive(true);
                qtySlider.wholeNumbers = true;
                qtySlider.onValueChanged.RemoveAllListeners();
                qtySlider.minValue = 1;
                qtySlider.maxValue = selectedItemSlot.quantity;
                qtySlider.value = 1;
                qtySliderLabel.text = qtySlider.value.ToString();

                qtySlider.onValueChanged.AddListener((val) =>
                {
                    qtySliderLabel.text = val.ToString();
                });
                qtySlider.wholeNumbers = true;
            }
            else
            {
                qtySlider.gameObject.SetActive(false);
            }
        }
        else
        {
            consumeButton.gameObject.SetActive(false);
            qtySlider.gameObject.SetActive(false);
        }
    }


    public void ConsumeItem()
    {
        if (selectedItemSlot == null || selectedItemSlot.slotItem == null)
            return;

        var item = selectedItemSlot.slotItem;
        int amountToConsume = selectedItemSlot.quantity > 1 ? Mathf.RoundToInt(qtySlider.value) : 1;

        
        if (item.itemPickedUpSFX != null) SFXManager.Instance.PlaySFX(item.itemUsedSFX);

        PlayerVitals.instance.RestoreHealth(item.healthEffect * amountToConsume);
        PlayerVitals.instance.RestoreStamina(item.healthEffect * amountToConsume);
        PlayerVitals.instance.ReplenishMagic(item.healthEffect * amountToConsume);

        selectedItemSlot.quantity -= amountToConsume;
        selectedItemSlot.UpdateQuantity(selectedItemSlot.quantity);

        if (selectedItemSlot.quantity <= 0)
        {
            var parentSlot = selectedItemSlot.transform.parent.GetComponent<ItemSlot>();
            if (parentSlot != null)
            {
                parentSlot.ClearSlot();
            }
            selectedItemSlot = null;
        }

        ShowConsumeButton(); 
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

        //Debug.LogWarning("No available inventory slot!");
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

    public bool IsInventoryFull()
    {
        foreach (var slot in inventoryItemSlots)
        {
            if (slot.inventoryItem == null ||
                (!slot.slotFilled && slot.heldItems < slot.maxHeldItems))
            {
                return false;
            }
        }
        return true;
    }
}
