using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class ItemCollectionQuestStep : QuestStep
{
    private int itemIdToCollect = 44;
    private int itemsCollected = 0;
    private int itemsToCollect = 5;

    private void Start()
    {
        stepDescription = "Collect 5 apples";
        stepName = "Apple Collection";
    }
    private void OnEnable()
    {
        GameEventsManager.instance.miscEvents.onItemCollected += OnItemCollected;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.miscEvents.onItemCollected -= OnItemCollected;
    }

    private void OnItemCollected(int itemId)
    {
        if (itemId == itemIdToCollect)
        {
            itemsCollected++;
            Debug.Log($"Picked up item {itemId}. Progress: {itemsCollected}/{itemsToCollect}");

            if (itemsCollected >= itemsToCollect)
            {
                Debug.Log("Quest Complete!");
                // Notify quest system here
                FinishQuestStep();
            }
        }
    }
}
