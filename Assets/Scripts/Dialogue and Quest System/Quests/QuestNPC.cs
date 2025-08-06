using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CircleCollider2D))]
public class QuestNPC : MonoBehaviour
{
    [Header("Quest Info")]
    [SerializeField] private List<QuestSO> quests = new(); // Multiple quests
    private int currentQuestIndex = 0;
    private Quest CurrentQuest => QuestManager.Instance.GetQuestById(CurrentQuestSO.id);
    private QuestSO CurrentQuestSO => quests[currentQuestIndex];

    private bool playerIsNear = false;

    [Header("Quest Config")]
    [SerializeField] private bool startPoint;
    [SerializeField] private bool endPoint;
    private QuestVisual questVisual;
    private DialogueManager dialogueManager;

    [Header("Quest Dialogue")]
    [SerializeField] private string canStartLine = "Hey, can you help me?";
    [SerializeField] private string inProgressLine = "Did you find what I asked for?";
    [SerializeField] private string canFinishLine = "You're back! Did you complete the task?";
    [SerializeField] private string finishedLine = "Thanks again!";

    private void Awake()
    {
        questVisual = GetComponentInChildren<QuestVisual>();
        dialogueManager = GetComponent<DialogueManager>();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
        GameEventsManager.instance.interactionEvents.onInteract += Interact;

        UpdateVisualAndState();
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
        GameEventsManager.instance.interactionEvents.onInteract -= Interact;
    }

    private void QuestStateChange(Quest quest)
    {
        if (quest.questSO == CurrentQuestSO)
        {
            UpdateVisualAndState();
        }
    }

    private void UpdateVisualAndState()
    {
        if (CurrentQuest != null)
        {
            questVisual.SetState(CurrentQuest.state, startPoint, endPoint);
        }
    }

    private void Interact(GameObject thisGo)
    {
        if (!playerIsNear || quests.Count == 0)
            return;

        var quest = CurrentQuest;

        if (quest == null) return;

        if (quest.state == QuestState.CAN_START && startPoint)
        {
            GameEventsManager.instance.questEvents.StartQuest(CurrentQuestSO.id);
        }
        else if (quest.state == QuestState.CAN_FINISH && endPoint)
        {
            GameEventsManager.instance.questEvents.FinishQuest(CurrentQuestSO.id);
            TryAdvanceToNextQuest();
        }

        ShowQuestDialogue();
    }

    private void TryAdvanceToNextQuest()
    {
        if (CurrentQuest.state == QuestState.FINISHED &&
            currentQuestIndex < quests.Count - 1)
        {
            currentQuestIndex++;
            UpdateVisualAndState();
        }
    }

    private void ShowQuestDialogue()
    {
        if (dialogueManager == null || CurrentQuest == null) return;

        string lineToSay = CurrentQuest.state switch
        {
            QuestState.CAN_START => canStartLine,
            QuestState.IN_PROGRESS => inProgressLine,
            QuestState.CAN_FINISH => canFinishLine,
            QuestState.FINISHED => finishedLine,
            _ => ""
        };

        dialogueManager.ShowText(lineToSay);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsNear = true;
            ShowQuestDialogue();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }
}
