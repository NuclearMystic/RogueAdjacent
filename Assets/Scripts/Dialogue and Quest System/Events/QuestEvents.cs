using UnityEngine;
using System;
public class QuestEvents
{
    public event Action<String> onQuestStart;

    public void StartQuest(String id)
    {
        if (onQuestStart != null)
        {
            onQuestStart(id);
        }
    }

    public event Action<String> onAdvanceQuest;

    public void AdvanceQuest(String id)
    {
        if (onAdvanceQuest != null)
        {
            onAdvanceQuest(id);
        }

    }

    public event Action<String> onFinishQuest;

    public void FinishQuest(String id)
    {
        if (onFinishQuest != null)
        {
            onFinishQuest(id);
        }
    }

    public event Action<Quest> onQuestStateChange;
    public void QuestStateChange(Quest quest)
    {
        if (onQuestStateChange != null)
        {
            onQuestStateChange(quest);
        }
    }

    public event Action<string> onQuestStepProgress;

    public void QuestStepProgressChanged(string questId)
    {
        if (onQuestStepProgress != null)
        {
            onQuestStepProgress(questId);
        }
    }

}
