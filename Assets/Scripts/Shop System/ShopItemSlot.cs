using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemSlot : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button buyButton;

    private InventoryItem itemData;
    private float itemPrice;

    public void Initialize(InventoryItem item)
    {
        itemData = item;
        itemPrice = ShopManager.Instance.GetPriceForItem(item);

        iconImage.sprite = itemData.ObjectIcon;
        nameText.text = itemData.ObjectName;
        priceText.text = itemPrice.ToString();

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => TryBuyItem());
    }

    public void TryBuyItem()
    {
        if (PlayerCurrencyManager.Instance.CanAfford(itemPrice))
        {
            PlayerCurrencyManager.Instance.Spend(itemPrice);

            bool added = false;

            // Clone the item so it's not a reference to the SO
            InventoryItem itemClone = ScriptableObject.Instantiate(itemData);
            PlayerInventoryManager.Instance.PickUpItem(itemClone, out added);
            UIManager.Instance.ForceRefreshCharacterMenu();

            if (added)
                Debug.Log($"Bought: {itemData.ObjectName} for {itemPrice} coins");
            else
                Debug.Log("Inventory full. Could not add item.");
        }
        else
        {
            Debug.Log("Not enough currency.");
        }
    }
}
