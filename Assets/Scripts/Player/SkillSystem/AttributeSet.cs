using System.Collections.Generic;

[System.Serializable]
public class AttributeSet
{
    public Dictionary<AttributeType, AttributeData> attributes = new();

    public AttributeSet()
    {
        foreach (AttributeType attr in System.Enum.GetValues(typeof(AttributeType)))
        {
            attributes[attr] = new AttributeData
            {
                attribute = attr,
                value = 1 
            };
        }
    }

    public AttributeData Get(AttributeType type)
    {
        if (!attributes.ContainsKey(type))
            attributes[type] = new AttributeData { attribute = type, value = 1 };
        return attributes[type];
    }

    public void Set(AttributeType type, int value)
    {
        if (!attributes.ContainsKey(type))
            attributes[type] = new AttributeData { attribute = type };
        attributes[type].value = value;
    }

    public bool ContainsKey(AttributeType type)
    {
        return attributes.ContainsKey(type);
    }
}
