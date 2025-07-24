using UnityEngine;

public class QuestVisual : MonoBehaviour
{
    [Header("Icons")]
    [SerializeField] private GameObject cantStart;
    [SerializeField] private GameObject canStart;
    [SerializeField] private GameObject inProgress;
    [SerializeField] private GameObject canFinish;

    public void SetState(QuestState newState, bool startPoint, bool endPoint)
    {
        cantStart.SetActive(false);
        canStart.SetActive(false);
        inProgress.SetActive(false);
        canFinish.SetActive(false);

        switch (newState)
        {
            case QuestState.REQUIREMENT_NOT_MET:
                if (startPoint) { cantStart.SetActive(true); }
                break;
            case QuestState.CAN_START:
                if (startPoint) { canStart.SetActive(true); }
                break;
            case QuestState.IN_PROGRESS:
                if (endPoint) { inProgress.SetActive(true); }
                break;
            case QuestState.CAN_FINISH:
                if (endPoint) { canFinish.SetActive(true); }
                break;
            case QuestState.FINISHED:
                break;
            default:
                Debug.LogWarning("Quest State not recognized by switch for quest icon: " + newState);
                break;
        }
    }
}
