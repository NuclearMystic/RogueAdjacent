using System;
using UnityEngine;

public class MiscEvents
{
    public event Action<int> onItemCollected;

    public void ItemCollected(int incomingItemID)
    {
        onItemCollected?.Invoke(incomingItemID);
    }

    public event Action<int, int> onItemSold;

    public void ItemSold(int incomingItemID, int incomingQty)
    {
        onItemSold?.Invoke(incomingItemID, incomingQty);
    }
}