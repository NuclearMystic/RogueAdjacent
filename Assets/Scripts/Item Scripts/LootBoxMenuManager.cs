using UnityEngine;
using UnityEngine.UI;

public class LootBoxMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button closeButton;
    [SerializeField] private ItemSlot[] lootSlots;

    private LootBoxManager currentBox;

    private void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            CloseLootBox();
        });

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
}
