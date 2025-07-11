using UnityEngine;

[System.Serializable]
public class SkillData
{
    public SkillType skill;

    public int value = 0; 
    public int level = 1;
    public float xp = 0f;

    [SerializeField] private float baseXP = 8f;
    [SerializeField] private float exponent = 1.01f;

    public float xpToNextLevel => baseXP * Mathf.Pow(level, exponent);

    public void LevelUp()
    {
        level++;
        xp = 0f;
        value = level;
    }
}
