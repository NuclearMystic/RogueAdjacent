using System;
using UnityEngine;

public class MiscEvents
{
    public event Action<int> onItemCollected;

    public void ItemCollected(int incomingItemID)
    {
        onItemCollected?.Invoke(incomingItemID);
    }

}