using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIObject : MonoBehaviour
{
    [SerializeField] private TMP_Text skillNameText;
    [SerializeField] private TMP_Text baseText;
    [SerializeField] private TMP_Text attributeText;
    [SerializeField] private TMP_Text totalText;
    [SerializeField] private Slider xpSlider;

    private SkillType skillType;
    private PlayerStats playerStats;

    private float lastXP = -1f;

    public void Initialize(SkillType skill, PlayerStats stats)
    {
        skillType = skill;
        playerStats = stats;

        int baseVal = stats.GetSkillBase(skill);
        int attrBonus = stats.GetSkillBonus(skill);
        int totalVal = stats.GetSkillTotal(skill);
        AttributeType attr = SkillAttributeMap.PrimaryAttribute[skill];

        skillNameText.text = skill.ToString();
        baseText.text = baseVal.ToString();
        attributeText.text = $"{attr} ({attrBonus})";
        totalText.text = totalVal.ToString();

        UpdateXPBar(); 
    }

    public void UpdateXPBar(bool force = false)
    {
        if (playerStats == null) return;

        SkillData skillData = playerStats.skills.Get(skillType);

        if (!force && Mathf.Approximately(skillData.xp, lastXP))
            return;

        xpSlider.maxValue = skillData.xpToNextLevel;
        xpSlider.value = skillData.xp;
        lastXP = skillData.xp;
    }
}
