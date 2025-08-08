using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;


public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    private Dictionary<string, Quest> questMap;
    [SerializeField] private int playerlevel = 0;

    public AudioClip questAccepted;
    public AudioClip questFinished;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        questMap = CreateQuestMap();
        DontDestroyOnLoad(gameObject);
        // Quest quest = GetQuestById("AppleCollectionQuest");
        // Debug.Log(quest.questSO.name + "\n" + quest.questSO.playerLevelRequirement + "\n" + quest.state + "\n" + quest.CurrentStepExists());
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.onQuestStart += StartQuest;
        GameEventsManager.instance.questEvents.onAdvanceQuest += AdvanceQuest;
        GameEventsManager.instance.questEvents.onFinishQuest += FinishQuest;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.onQuestStart -= StartQuest;
        GameEventsManager.instance.questEvents.onAdvanceQuest -= AdvanceQuest;
        GameEventsManager.instance.questEvents.onFinishQuest -= FinishQuest;
    }

    private void Start()
    {
        foreach (Quest quest in questMap.Values)
        {
            GameEventsManager.instance.questEvents.QuestStateChange(quest);
        }
    }

    private void Update()
    {
        UpdateAllQuests();
    }

    private void UpdateAllQuests()
    {
        foreach (Quest quest in questMap.Values)
        {
            if (quest.state == QuestState.REQUIREMENT_NOT_MET && CheckRequirementsMet(quest))
            {
                ChangeQuestState(quest.questSO.id, QuestState.CAN_START);
            }
        }
    }
    private bool CheckRequirementsMet(Quest quest)
    {
        bool meetsRequirements = true;

        if (playerlevel < quest.questSO.playerLevelRequirement)
        {
            meetsRequirements = false;
        }

        foreach (QuestSO questPrereq in quest.questSO.questPrereqs)
        {
            if (GetQuestById(questPrereq.id).state != QuestState.FINISHED)
            {
                meetsRequirements = false;
            }
        }

        return meetsRequirements;
    }

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestById(id);
        quest.state = state;
        GameEventsManager.instance.questEvents.QuestStateChange(quest);
    }
    private void StartQuest(string id)
    {
        Quest quest = GetQuestById(id);
        quest.InstantiateCurrentQuestStep(this.transform);
        ChangeQuestState(quest.questSO.id, QuestState.IN_PROGRESS);
        SFXManager.Instance.PlaySFX(questAccepted, 1.2f);
    }
    private void AdvanceQuest(string id)
    {
        Quest quest = GetQuestById(id);
        quest.MoveToNextStep();

        if (quest.CurrentStepExists())
        {
            quest.InstantiateCurrentQuestStep(this.transform);
        }
        else
        {
            ChangeQuestState(quest.questSO.id, QuestState.CAN_FINISH);
        }
    }

    private void FinishQuest(string id)
    {
        Quest quest = GetQuestById(id);
        ClaimRewards(quest);
        ChangeQuestState(quest.questSO.id, QuestState.FINISHED);
        SFXManager.Instance.PlaySFX(questFinished);
    }

    private void ClaimRewards(Quest quest)
    {
        GameEventsManager.instance.currencyEvents.CurrencyGained(quest.questSO.goldReward);
    }

    private Dictionary<string, Quest> CreateQuestMap()
    {
        QuestSO[] allQuests = Resources.LoadAll<QuestSO>("Quests");

        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
        foreach (QuestSO questSO in allQuests)
        {
            if (idToQuestMap.ContainsKey(questSO.id))
            {
                Debug.LogWarning("Duplicate quest ID found");
            }
            idToQuestMap.Add(questSO.id, new Quest(questSO));
        }
        return idToQuestMap;
    }

    public Quest GetQuestById(string id)
    {
        Quest quest = questMap[id];
        if (quest == null)
        {
            Debug.LogWarning("Quest ID not found in Quest Map" + id);
        }
        return quest;
    }

    public List<Quest> GetVisibleQuests()
    {
        List<Quest> result = new List<Quest>();
        foreach (var quest in questMap.Values)
        {
            if (quest.state == QuestState.IN_PROGRESS || quest.state == QuestState.CAN_FINISH)
            {
                result.Add(quest);
            }
        }
        return result;
    }
}
