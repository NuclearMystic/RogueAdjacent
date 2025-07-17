using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SkillAttributeMap
{
    public static Dictionary<SkillType, AttributeType> PrimaryAttribute = new()
    {
        { SkillType.None, AttributeType.None },
        { SkillType.Axes, AttributeType.STR },
        { SkillType.Maces, AttributeType.STR },
        { SkillType.Swords, AttributeType.STR },
    };

    public static int GetAttributeBonus(SkillType type, AttributeSet attributes)
    {
        var attr = PrimaryAttribute[type];
        int bonus = attributes.Get(attr).value;

        return bonus;
    }

    public static AttributeType GetPrimaryAttribute(SkillType type)
    {
        return PrimaryAttribute.TryGetValue(type, out var attr) ? attr : AttributeType.None;
    }

}

