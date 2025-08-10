using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ShopTab { Weapon, Armor, Food, Sell }

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Transform shopSlotParent;
    [SerializeField] private GameObject shopSlotPrefab;

    [Header("Sell Tab References")]
    [SerializeField] public Transform sellSlotParent;
    [SerializeField] private TextMeshProUGUI totalSellValueText;
    [SerializeField] private Button sellButton;
    [SerializeField] private Button cancelSellButton;
    [SerializeField] private GameObject shopTabPageContainer;
    [SerializeField] private GameObject sellTabPageContainer;

    [Header("Tab Buttons")]
    [SerializeField] private Button weaponTabButton;
    [SerializeField] private Button armorTabButton;
    [SerializeField] private Button foodTabButton;
    [SerializeField] private Button sellTabButton;
    [SerializeField] private Button closeButton;

    [Header("Shop Data")]
    [SerializeField] private ShopInfoSO currentShop;

    private List<DraggableIconSlot> itemsToSell = new();
    private float totalSellValue = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        shopPanel = this.gameObject;
        shopPanel.SetActive(false);
    }

    private void Start()
    {
        weaponTabButton.onClick.AddListener(() => ShowTab(ShopTab.Weapon));
        armorTabButton.onClick.AddListener(() => ShowTab(ShopTab.Armor));
        foodTabButton.onClick.AddListener(() => ShowTab(ShopTab.Food));
        sellTabButton.onClick.AddListener(() => ShowTab(ShopTab.Sell));
        closeButton.onClick.AddListener(CloseWindow);
        sellButton.onClick.AddListener(ProcessSale);
        cancelSellButton.onClick.AddListener(CancelSale);
    }

    public void OpenShop(ShopInfoSO shopData)
    {
        currentShop = shopData;
        shopPanel.SetActive(true);
        ShowTab(ShopTab.Weapon);
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        ClearShopSlots();
    }

    public void CloseWindow()
    {
        shopPanel.SetActive(false);
    }

    public void ShowTab(ShopTab tab)
    {
        ClearShopSlots();
        itemsToSell.Clear();
        UpdateTotalSellValue();

        shopTabPageContainer.SetActive(tab != ShopTab.Sell);
        sellTabPageContainer.SetActive(tab == ShopTab.Sell);

        if (tab == ShopTab.Sell) return;

        foreach (var entry in currentShop.itemsForSale)
        {
            if (MatchesTab(entry.item.itemType, tab))
            {
                CreateShopSlot(entry.item);
            }
        }
    }

    public void RegisterSellItemViaGhost(InventoryItem item, int qty)
    {
        if (item == null)
        {
            Debug.LogWarning("Tried to register null item via ghost.");
            return;
        }

        if (item.draggableIcon == null)
        {
            Debug.LogWarning($"Item '{item.ObjectName}' has no draggableIcon prefab.");
            return;
        }

        if (sellSlotParent == null)
        {
            Debug.LogError("Sell slot parent is not set in ShopManager.");
            return;
        }

        GameObject newSlot = Instantiate(item.draggableIcon, sellSlotParent);
        DraggableIconSlot icon = newSlot.GetComponent<DraggableIconSlot>();

        if (icon == null)
        {
            Debug.LogError("Instantiated item does not have DraggableIconSlot component.");
            return;
        }

        icon.slotItem = item;
        icon.SetQuantity(qty);

        RegisterSellItem(icon);
    }

    private void CreateShopSlot(InventoryItem item)
    {
        GameObject slotGO = Instantiate(shopSlotPrefab, shopSlotParent);
        ShopItemSlot slot = slotGO.GetComponent<ShopItemSlot>();
        slot.Initialize(item);
    }

    private void ClearShopSlots()
    {
        foreach (Transform child in shopSlotParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in sellSlotParent)
        {
            Destroy(child.gameObject);
        }
    }

    private bool MatchesTab(InventoryItem.SlotType type, ShopTab tab)
    {
        return tab switch
        {
            ShopTab.Weapon => type == InventoryItem.SlotType.Weapon,
            ShopTab.Food => type == InventoryItem.SlotType.Food,
            ShopTab.Armor => type == InventoryItem.SlotType.Hat ||
                             type == InventoryItem.SlotType.Cape ||
                             type == InventoryItem.SlotType.Outfit ||
                             type == InventoryItem.SlotType.FaceAcces,
            _ => false
        };
    }

    public float GetPriceForItem(InventoryItem item)
    {
        return Mathf.Ceil(item.baseCost * currentShop.buyMarkup);
    }

    public float GetSellValueForItem(InventoryItem item)
    {
        return Mathf.Ceil(item.baseCost * currentShop.sellMultiplier);
    }

    public void RegisterSellItem(DraggableIconSlot itemSlot)
    {
        if (itemSlot == null) return;

        if (!itemsToSell.Contains(itemSlot))
        {
            itemsToSell.Add(itemSlot);
        }

        LateCleanupSellList();
    }

    public void UnregisterSellItem(DraggableIconSlot itemSlot)
    {
        if (itemSlot == null) return;

        if (itemsToSell.Contains(itemSlot))
        {
            itemsToSell.Remove(itemSlot);
            UpdateTotalSellValue();
        }
    }

    private void LateCleanupSellList()
    {
        StartCoroutine(ILateCleanup());
    }

    private IEnumerator ILateCleanup()
    {
        yield return null;

        itemsToSell.RemoveAll(slot =>
            slot == null ||
            slot.slotItem == null ||
            slot.transform == null ||
            slot.transform.parent != sellSlotParent);

        UpdateTotalSellValue();
    }

    private void UpdateTotalSellValue()
    {
        totalSellValue = 0;
        foreach (var slot in itemsToSell)
        {
            totalSellValue += GetSellValueForItem(slot.slotItem) * slot.quantity;
        }

        totalSellValueText.text = totalSellValue.ToString();
    }

    private void ProcessSale()
    {
        foreach (var slot in itemsToSell)
        {
            var item = slot.slotItem;
            int qty = slot.quantity;

            // 1) pay player
            float amount = GetSellValueForItem(item) * qty;
            GameEventsManager.instance.currencyEvents.CurrencyGained(amount);
            GameEventsManager.instance.miscEvents.ItemSold(item.itemId, qty);

            // 2) remove from inventory NOW
            PlayerInventoryManager.Instance.RemoveItemsById(item.itemId, qty);

            // 3) UI cleanup
            Destroy(slot.gameObject);
        }

        itemsToSell.Clear();
        UpdateTotalSellValue();
    }

    private void CancelSale()
    {
        foreach (var slot in new List<DraggableIconSlot>(itemsToSell))
        {
            if (slot != null) Destroy(slot.gameObject);
        }
        itemsToSell.Clear();
        UpdateTotalSellValue();
    }
}
