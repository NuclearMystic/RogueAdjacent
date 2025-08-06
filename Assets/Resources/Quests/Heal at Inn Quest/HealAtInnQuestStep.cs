using UnityEngine;

public class HealAtInnQuestStep : QuestStep
{ 

    private void Start()
    {
        stepName = "Heal At Inn";
        stepDescription = "Use the Inn in town to heal up!";
    }

    private void OnEnable()
    {
        GameEventsManager.instance.playerEvents.onHealedAtInn += OnHealedAtInn;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.playerEvents.onHealedAtInn -= OnHealedAtInn;
    }

    private void OnHealedAtInn(bool healedAtInn)
    {
        if (healedAtInn)
        {
            FinishQuestStep();
        }
    }

}
