using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    public static event Action<SkillType> OnSkillChanged;

    public AttributeSet attributes = new();
    public SkillSet skills = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public int GetSkillBase(SkillType skill) => skills.Get(skill).value;

    public int GetSkillBonus(SkillType skill)
        => SkillAttributeMap.GetAttributeBonus(skill, attributes);

    public int GetSkillTotal(SkillType skill)
        => GetSkillBase(skill) + GetSkillBonus(skill);

    public void ModifySkill(SkillType skill, int delta)
    {
        SkillData data = skills.Get(skill);
        data.value += delta;

        OnSkillChanged?.Invoke(skill);
    }

    public bool AddSkillXP(SkillType skill, float xp)
    {
        if (!skills.ContainsKey(skill)) return false;

        SkillData data = skills.Get(skill);
        data.xp += xp;

        if (data.xp >= data.xpToNextLevel)
        {
            data.LevelUp();
            Debug.Log($"Skill {skill} leveled up to {data.level}!");
            OnSkillChanged?.Invoke(skill);
            return true;
        }

        return false;
    }

    public bool AddAttributeXP(AttributeType attribute, float xp)
    {
        if (!attributes.ContainsKey(attribute)) return false;

        AttributeData data = attributes.Get(attribute);

        float dampenedXP = ApplyAttributeXPDampening(xp, data.level);
        data.xp += dampenedXP;

        if (data.xp >= data.xpToNextLevel)
        {
            data.LevelUp();
            Debug.Log($"Attribute {attribute} leveled up to {data.level}!");
            return true;
        }

        return false;
    }

    private float ApplyAttributeXPDampening(float rawXP, int currentLevel)
    {
        float dampeningFactor = 1f / Mathf.Pow(currentLevel, 1.25f);
        return rawXP * dampeningFactor;
    }


}
