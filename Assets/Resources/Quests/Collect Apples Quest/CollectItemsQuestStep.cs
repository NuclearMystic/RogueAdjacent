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
            InGameConsole.Instance.SendMessageToConsole($"Picked up item {itemId}. Progress: {itemsCollected}/{itemsToCollect}");
            UpdateStepDescription();
            if (itemsCollected >= itemsToCollect)
            {
                InGameConsole.Instance.SendMessageToConsole("Quest Complete!");
                // Notify quest system here
                FinishQuestStep();
            }
        }
    }

    private void UpdateStepDescription()
    {
        stepDescription = $"Collect {itemsToCollect} apples ({itemsCollected}/{itemsToCollect})";
        GameEventsManager.instance.questEvents.QuestStepProgressChanged(questId);
    }
}
