using System.Collections.Generic;
using UnityEngine;

public class SkillMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject skillPrefab;
    [SerializeField] private Transform skillGridParent;
    [SerializeField] private PlayerStats playerStats;


    private Dictionary<SkillType, SkillUIObject> skillUIMap = new();

    private void OnEnable()
    {
        PlayerStats.OnSkillChanged += RefreshSkill;
    }

    private void OnDisable()
    {
        PlayerStats.OnSkillChanged -= RefreshSkill;
    }

    public void RefreshAllSkills()
    {
        foreach (var kvp in skillUIMap)
        {
            kvp.Value.Initialize(kvp.Key, playerStats);
        }
    }

    private void Start()
    {
        PopulateSkillMenu();
    }

    private void PopulateSkillMenu()
    {
        skillUIMap.Clear();

        foreach (SkillType skill in System.Enum.GetValues(typeof(SkillType)))
        {
            GameObject skillGO = Instantiate(skillPrefab, skillGridParent);
            SkillUIObject skillUI = skillGO.GetComponent<SkillUIObject>();
            skillUI.Initialize(skill, playerStats);

            skillUIMap.Add(skill, skillUI);
        }
    }

    private void RefreshSkill(SkillType skill)
    {
        if (skillUIMap.TryGetValue(skill, out var skillUI))
        {
            skillUI.UpdateXPBar();
        }
    }
}
