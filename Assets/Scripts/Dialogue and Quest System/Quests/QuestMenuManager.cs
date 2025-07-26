using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestMenuManager : MonoBehaviour
{
    public static QuestMenuManager Instance { get; private set; }

    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject thisPanel;
    [SerializeField] private Transform contentContainer;
    [SerializeField] private GameObject baseQuestItem;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }


        Instance = this;
        thisPanel.SetActive(false);
    }

    private void Start()
    {
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() => CloseMenu());
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange += OnQuestChanged;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange -= OnQuestChanged;
    }

    private void OnQuestChanged(Quest quest)
    {
        if (!thisPanel.activeSelf) return;

        StartCoroutine(DelayedRefresh());
    }

    public void OpenMenu()
    {
        thisPanel.SetActive(true);
        RefreshQuestList();
    }

    public void CloseMenu()
    {
        thisPanel.SetActive(false);
    }

    public void RefreshQuestList()
    {

        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }

        QuestManager questManager = FindFirstObjectByType<QuestManager>();
        if (questManager == null)
        {
            Debug.LogWarning("QuestManager not found in scene.");
            return;
        }

        List<Quest> quests = questManager.GetVisibleQuests();
        foreach (Quest quest in quests)
        {
            GameObject itemGO = Instantiate(baseQuestItem, contentContainer);
            QuestUIObject questUI = itemGO.GetComponent<QuestUIObject>();
            if (questUI != null)
            {
                questUI.SetQuestData(quest);
            }
        }


        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentContainer);
    }

    private IEnumerator DelayedRefresh()
    {
        yield return null; // wait 1 frame
        RefreshQuestList();
    }
}
