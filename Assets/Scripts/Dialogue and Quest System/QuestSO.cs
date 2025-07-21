using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "QuestSO", menuName = "Quests System/QuestSO", order = 1)]

public class QuestSO : ScriptableObject
{
    [field: SerializeField] public string id {  get; private set; }

    [Header("Quest Info")]
    public string questName;

    [Header("Quest Pre-requirements")]
    public int playerLevelRequirement;
    public QuestSO[] questPrereqs;

    [Header("Quest Steps")]
    public GameObject[] questSteps;

    [Header("Rewards")]
    public int goldReward;
    public int experienceReward;

    private void OnValidate()
    {
        #if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }

}
