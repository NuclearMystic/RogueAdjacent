using UnityEngine;

[CreateAssetMenu(menuName = "Interaction System/Healing Interaction")]
public class HealingInteraction : InteractionSO
{
    public int healAmount;
    public int healCost;
    public bool fullRestoration;

    public override void Execute(GameObject actor, InteractableGameObject target)
    {
        PlayerVitals vitals = PlayerVitals.instance;
        if (vitals == null)
        {
            Debug.LogWarning("PlayerVitals component not found on actor.");
            return;
        }

        float currentCurrency = PlayerCurrencyManager.Instance.GetCurrency();

        if (currentCurrency < healCost)
        {
            InGameConsole.Instance.SendMessageToConsole("Not enough gold to rest at the inn.");
            return;
        }

        PlayerCurrencyManager.Instance.Spend(healCost);

        if (fullRestoration)
        {
            vitals.RestoreAllVitals();
        }
        else
        {
            vitals.RestoreHealth(healAmount);
            vitals.RestoreStamina(healAmount);
            vitals.ReplenishMagic(healAmount);
        }

        GameEventsManager.instance.playerEvents.HealedAtInn(true);
        InGameConsole.Instance.SendMessageToConsole($"Rested at the inn. Spent {healCost} gold and restored {healAmount} to all vitals.");
    }
    public override string GetPromptText()
    {
        return $"Rest at inn ({healCost} gold) - Heal {healAmount}";
    }
}
