using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop System/Shop Info")]
public class ShopInfoSO : ScriptableObject
{
    [Header("Shop Info")]
    public string shopName;
    public Sprite shopIcon;

    [Header("Pricing")]
    public float buyMarkup = 1.0f;
    public float sellMultiplier = 0.5f;

    [Header("Inventory")]
    public List<ShopItemEntry> itemsForSale = new List<ShopItemEntry>();
}

[System.Serializable]
public struct ShopItemEntry
{
    public InventoryItem item;
    public int quantity;
    public bool isRestocking;
}
