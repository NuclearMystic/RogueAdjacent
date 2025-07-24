using UnityEngine;

[System.Serializable]
public class AttributeData
{
    public AttributeType attribute;

    public int level = 1;
    public float xp = 0f;
    public int value = 1; 

    [SerializeField] private float baseXP = 50f;
    [SerializeField] private float exponent = 1.75f;

    public float xpToNextLevel => baseXP * Mathf.Pow(level, exponent);

    public void LevelUp()
    {
        level++;
        xp = 0f;
        value++; 
        Debug.Log($"[Attribute Level Up] {attribute} is now level {level}");
    }
}
