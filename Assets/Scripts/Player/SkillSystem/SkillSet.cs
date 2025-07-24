using System.Collections.Generic;

[System.Serializable]
public class SkillSet
{
    public Dictionary<SkillType, SkillData> skills = new();

    public SkillSet()
    {
        foreach (SkillType skill in System.Enum.GetValues(typeof(SkillType)))
        {
            skills[skill] = new SkillData
            {
                skill = skill,
                value = 1
            };
        }
    }

    public SkillData Get(SkillType type)
    {
        if (!skills.ContainsKey(type))
        {
            skills[type] = new SkillData { skill = type, value = 1 };
        }
        return skills[type];
    }

    public void Set(SkillType type, int value)
    {
        if (!skills.ContainsKey(type))
        {
            skills[type] = new SkillData { skill = type };
        }
        skills[type].value = value;
    }

    public bool ContainsKey(SkillType type)
    {
        return skills.ContainsKey(type);
    }
}
