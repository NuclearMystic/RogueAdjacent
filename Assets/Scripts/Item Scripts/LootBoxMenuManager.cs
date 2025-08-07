using UnityEngine;
using UnityEngine.UI;

public class LootBoxMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button closeButton;
    [SerializeField] private ItemSlot[] lootSlots;
    [SerializeField] private Button takeAllButton;
    private LootBoxManager currentBox;

    private void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            CloseLootBox();
        });

        takeAllButton.onClick.AddListener(() => { TakeAll(); });    

        panel.SetActive(false);
    }

    public void OpenLootBox(LootBoxManager box, LootTable table)
    {
        currentBox = box;

        panel.SetActive(true);

        // Clear all previous items
        foreach (var slot in lootSlots)
        {
            slot.ClearSlot();
        }

        // Fill slots with random loot
        for (int i = 0; i < lootSlots.Length; i++)
        {
            InventoryItem item = table.GetRandomLootItem();
            if (item != null)
            {
                lootSlots[i].ReceiveInventoryItem(item, 1);
            }
        }
    }

    public void CloseLootBox()
    {
        panel.SetActive(false);

        if (currentBox != null)
        {
            currentBox.OnClose();
            currentBox = null;
        }
    }

    public void TakeAll()
    {
        foreach (var slot in lootSlots)
        {
            if (slot.HasItem())
            {
                InventoryItem item = slot.inventoryItem;
                int quantity = slot.heldItems;

                InventoryManager.Instance.AddItemToInventoryWithQuantity(item, quantity);

                slot.ClearSlot();
            }
        }

        CloseLootBox();
    }
}
