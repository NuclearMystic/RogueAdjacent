using UnityEngine;
using TMPro;

public class QuestUIObject : MonoBehaviour
{
    [SerializeField] private TMP_Text questTitle;
    [SerializeField] private TMP_Text questStepTitle;
    [SerializeField] private TMP_Text questStepDesc;
    [SerializeField] private TMP_Text questStateLabel;

    private Quest quest;

    public void SetQuestData(Quest quest)
    {
        this.quest = quest;
        questTitle.text = quest.questSO.questName;
        Debug.Log(quest.GetCurrentStepName());
        questStepTitle.text = quest.GetCurrentStepName();
        Debug.Log(quest.GetCurrentStepDescription());
        questStepDesc.text = quest.GetCurrentStepDescription();
        questStateLabel.text = quest.state.ToString();

    }

}
