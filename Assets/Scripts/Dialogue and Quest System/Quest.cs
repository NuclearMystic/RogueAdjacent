using UnityEngine;

public class Quest
{
    public QuestSO questSO;
    public QuestState state;
    private int currentQuestStepIndex;
    private QuestStep activeStepInstance;

    public Quest(QuestSO questSOin)
    {
        this.questSO = questSOin;
        this.state = QuestState.REQUIREMENT_NOT_MET;
        this.currentQuestStepIndex = 0;
    }

    public void MoveToNextStep()
    {
        currentQuestStepIndex++;
    }

    public bool CurrentStepExists()
    {
        return (currentQuestStepIndex < questSO.questSteps.Length);
    }

    public void InstantiateCurrentQuestStep(Transform parentTransform)
    {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();
        if (questStepPrefab != null)
        {
            QuestStep questStep = Object.Instantiate(questStepPrefab, parentTransform).GetComponent<QuestStep>();
            questStep.InitializeQuestStep(questSO.id);
            activeStepInstance = questStep;
        }
    }

    private GameObject GetCurrentQuestStepPrefab()
    {
        GameObject questStepPrefab = null;
        if (CurrentStepExists()) { 
            questStepPrefab = questSO.questSteps[currentQuestStepIndex];
        }
        else
        {
            Debug.LogWarning("Quest step prefab is out of range");
        }

        return questStepPrefab; 
    }

    public string GetCurrentStepName()
    {
        return activeStepInstance != null ? activeStepInstance.GetStepName() : "";
    }

    public string GetCurrentStepDescription()
    {
        return activeStepInstance != null ? activeStepInstance.GetStepDescription() : "";
    }
}
