using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;
    private string questId;


    protected string stepName;
    protected string stepDescription;

    public void InitializeQuestStep(string questId)
    {
        this.questId = questId;

    }
    protected void FinishQuestStep()
    {
        if (!isFinished)
        {
            isFinished = true;
            GameEventsManager.instance.questEvents.AdvanceQuest(questId);

            Destroy(this.gameObject);
        }
    }

    public string GetStepName() => stepName;
    public string GetStepDescription() => stepDescription;
}

