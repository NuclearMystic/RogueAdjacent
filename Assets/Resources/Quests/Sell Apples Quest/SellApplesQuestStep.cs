using UnityEngine;

public class SellApplesQuestStep : QuestStep
{
    private int itemIDToSell = 44;
    private int itemsToSell = 5;
    private int itemsSold = 0;

    private void Start()
    {
        stepName = "Sell Apples";
        stepDescription = "Find the merchant infront of the blacksmith and sell him apples.";
    }

    private void OnEnable()
    {
        GameEventsManager.instance.miscEvents.onItemSold += OnItemSold;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.miscEvents.onItemSold -= OnItemSold;
    }

    private void OnItemSold(int itemId, int qty)
    {
        if (itemIDToSell == itemId)
        {
            itemsSold += qty;
            InGameConsole.Instance.SendMessageToConsole($"Sold item {itemId}. Quest Progress: {itemsSold}/{itemsToSell}");
            UpdateStepDescription();
        }
        if (itemsSold >= itemsToSell) { 
            FinishQuestStep();
        }
    }

    private void UpdateStepDescription()
    {
        stepDescription = $"Sell {itemsToSell} apples ({itemsSold}/{itemsToSell})";
        GameEventsManager.instance.questEvents.QuestStepProgressChanged(questId);
    }
}
