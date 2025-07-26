using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

[RequireComponent(typeof(CircleCollider2D))]
public class QuestNPC : MonoBehaviour
{
    [Header("Quest Info")]
    [SerializeField] private QuestSO questSO;
    private bool playerIsNear = false;

    private string questId;
    private QuestState currentQuestState;

    [Header("Quest Config")]
    [SerializeField] private bool startPoint;
    [SerializeField] private bool endPoint;
    private QuestVisual questVisual;

    [Header("Quest Dialogue")]
    [SerializeField] private string canStartLine = "Hey, can you help me?";
    [SerializeField] private string inProgressLine = "Did you find what I asked for?";
    [SerializeField] private string canFinishLine = "You're back! Did you complete the task?";
    [SerializeField] private string finishedLine = "Thanks again!";
    private DialogueManager dialogueManager;


    private void Awake()
    {
        questId = questSO.id;
        questVisual = GetComponentInChildren<QuestVisual>();
        dialogueManager = GetComponent<DialogueManager>();
        //DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
        GameEventsManager.instance.interactionEvents.onInteract += Interact;

        if (QuestManager.Instance != null)
        {
            Quest quest = QuestManager.Instance.GetQuestById(questId);
            QuestStateChange(quest);
        }
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
        GameEventsManager.instance.interactionEvents.onInteract -= Interact;
    }

    private void QuestStateChange(Quest quest)
    {
        if (quest.questSO.id.Equals(questId))
        {
            currentQuestState = quest.state;
            questVisual.SetState(currentQuestState, startPoint, endPoint);
        }
    }
    private void Interact(GameObject thisGo)
    {
        if (!playerIsNear)
        {
            return;
        }

        if (currentQuestState.Equals(QuestState.CAN_START) && startPoint)
        {
            GameEventsManager.instance.questEvents.StartQuest(questId);
        }
        else if (currentQuestState.Equals(QuestState.CAN_FINISH) && endPoint)
        {
            GameEventsManager.instance.questEvents.FinishQuest(questId);
        }
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

    private void ShowQuestDialogue()
    {
        if (dialogueManager == null) return;

        Quest quest = QuestManager.Instance.GetQuestById(questSO.id); // however you're accessing the quest

        string lineToSay = "";

        switch (quest.state)
        {
            case QuestState.CAN_START:
                lineToSay = canStartLine;
                break;
            case QuestState.IN_PROGRESS:
                lineToSay = inProgressLine;
                break;
            case QuestState.CAN_FINISH:
                lineToSay = canFinishLine;
                break;
            case QuestState.FINISHED:
                lineToSay = finishedLine;
                break;
        }

        dialogueManager.ShowText(lineToSay);
    }

}
