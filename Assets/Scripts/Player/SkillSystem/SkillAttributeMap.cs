using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SkillAttributeMap
{
    public static Dictionary<SkillType, AttributeType> PrimaryAttribute = new()
    {
        { SkillType.OneHandedSword, AttributeType.STR }, 
        { SkillType.TwoHandedSword, AttributeType.STR },
        { SkillType.OneHandedAxe, AttributeType.STR },
        { SkillType.TwoHandedAxe, AttributeType.STR },
        { SkillType.OneHandedClub, AttributeType.STR },
        { SkillType.TwoHandedClub, AttributeType.STR },
        { SkillType.Daggers, AttributeType.AGI },
        { SkillType.BowAndArrow, AttributeType.PER },
        { SkillType.LightArmor, AttributeType.AGI },
        { SkillType.HeavyArmor, AttributeType.END },
        { SkillType.Staves, AttributeType.INT },
        { SkillType.Wands, AttributeType.MEM },
        { SkillType.ElementalMagic, AttributeType.INT },
        { SkillType.Alchemy, AttributeType.INT },
        { SkillType.Enchanting, AttributeType.MEM },
        { SkillType.Cooking, AttributeType.MEM },
        { SkillType.Fishing, AttributeType.PER },
        { SkillType.Farming, AttributeType.END },
        { SkillType.Blacksmithing, AttributeType.END },
        { SkillType.Barter, AttributeType.CHA },
        { SkillType.Persuasion, AttributeType.INT },
        { SkillType.Intimidate, AttributeType.STR },
        { SkillType.Lie, AttributeType.MEM },
        { SkillType.Acrobatics, AttributeType.AGI },
        { SkillType.Athletics, AttributeType.END }, 
        { SkillType.Awareness, AttributeType.PER },
        { SkillType.Sneaking, AttributeType.PER }
    };

    public static bool IsHybrid(SkillType type)
    {
        return type == SkillType.OneHandedSword || type == SkillType.OneHandedAxe || type == SkillType.Athletics;
    }

    public static int GetAttributeBonus(SkillType type, AttributeSet attributes)
    {
        if (IsHybrid(type))
        {
            if (type == SkillType.Athletics)
                return Mathf.Max(attributes.Get(AttributeType.AGI).value, attributes.Get(AttributeType.END).value);
            else // 1-Handed Sword or Axe
                return Mathf.Max(attributes.Get(AttributeType.STR).value, attributes.Get(AttributeType.AGI).value);
        }

        var attr = PrimaryAttribute[type];
        int bonus = attributes.Get(attr).value;

        // Add Charisma bonus for social skills not primarily using it
        if ((type == SkillType.Persuasion && attr != AttributeType.CHA) ||
            (type == SkillType.Intimidate && attr != AttributeType.CHA) ||
            (type == SkillType.Lie && attr != AttributeType.CHA))
        {
            // Optional secondary scaling
            bonus += Mathf.RoundToInt(attributes.Get(AttributeType.CHA).value * 0.2f);
        }

        return bonus;
    }

    public static AttributeType GetPrimaryAttribute(SkillType type)
    {
        return PrimaryAttribute.TryGetValue(type, out var attr) ? attr : AttributeType.None;
    }

}

