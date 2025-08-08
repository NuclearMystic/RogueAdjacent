using UnityEngine;

public class LootBoxManager : MonoBehaviour
{
    [SerializeField] private Sprite closedBox;
    [SerializeField] private Sprite openBox;
    [SerializeField] private LootTable lootTable;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private LootBoxMenuManager lootBoxMenu;

    [Header("Loot Settings")]
    [SerializeField] private int numberOfItemsToGenerate = 3;

    public bool hasBeenOpened = false;

    private void Start()
    {
        if (lootTable == null)
            lootTable = FindAnyObjectByType<LootTable>();

        if (lootBoxMenu == null)
            lootBoxMenu = FindAnyObjectByType<LootBoxMenuManager>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateSprite();
    }

    public void OnOpen()
    {
        if (hasBeenOpened)
            return;

        hasBeenOpened = true;

        InventoryItem[] loot = new InventoryItem[numberOfItemsToGenerate];
        for (int i = 0; i < numberOfItemsToGenerate; i++)
        {
            loot[i] = lootTable.GetRandomLootItem();
        }

        lootBoxMenu.OpenLootBox(this, lootTable);
        UpdateSprite();
    }

    public void OnClose()
    {
        spriteRenderer.sprite = closedBox;
        GetComponent<Collider2D>().enabled = false;

    }

    private void UpdateSprite()
    {
        if (spriteRenderer == null) return;

        if (hasBeenOpened)
        {
            spriteRenderer.sprite = openBox;
            spriteRenderer.color = Color.grey; // Tint to grey when used
        }
        else
        {
            spriteRenderer.sprite = closedBox;
            spriteRenderer.color = Color.white; // Default tint
        }
    }
}
