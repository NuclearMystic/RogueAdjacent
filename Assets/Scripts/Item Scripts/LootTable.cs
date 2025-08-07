using UnityEngine;

public class LootTable : MonoBehaviour
{
    public InventoryItem[] itemsDatabase;
    private InventoryItem[] cachedLoot;
    public InventoryItem GetRandomLootItem()
    {
        if (itemsDatabase == null || itemsDatabase.Length == 0)
        {
            Debug.LogWarning("LootTable is empty!");
            return null;
        }
        
        int attempts = 10;

        for (int i = 0; i < attempts; i++)
        {
            int randomIndex = Random.Range(0, itemsDatabase.Length);
            InventoryItem candidate = itemsDatabase[randomIndex];

            if (candidate != null && candidate.itemPrefab != null && candidate.draggableIcon != null)
            {
                return candidate;
            }
        }

        Debug.LogWarning("No valid loot item found in LootTable after multiple attempts.");
        return null;
    }
    

}
