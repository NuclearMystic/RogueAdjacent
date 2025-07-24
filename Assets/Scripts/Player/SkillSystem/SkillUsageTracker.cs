public static class SkillUsageTracker
{
    private const float AttributeXPPercent = 0.05f;

    public static void RegisterSkillUse(SkillType skill, float xpAmount)
    {
        if (PlayerStats.Instance == null) return;

        bool skillLeveledUp = PlayerStats.Instance.AddSkillXP(skill, xpAmount);
        if (skillLeveledUp)
        {
            // Show skill level up feedback
            InGameConsole.Instance.SendMessageToConsole($"{skill} leveled up!");

            // Only give attribute XP if the skill actually leveled up
            AttributeType governingAttribute = SkillAttributeMap.GetPrimaryAttribute(skill);
            if (governingAttribute != AttributeType.None)
            {
                float attributeXP = xpAmount * AttributeXPPercent;
                bool attributeLeveledUp = PlayerStats.Instance.AddAttributeXP(governingAttribute, attributeXP);

                if (attributeLeveledUp)
                {
                    InGameConsole.Instance.SendMessageToConsole($"{governingAttribute} leveled up!");
                }
            }
        }

        //Refresh skill list when menu opens and closes to show most recent updates to skills
        if (UIManager.Instance.skillsMenuOpen == true)
        {
            UIManager.Instance.gameObject.GetComponent<SkillMenuUI>().RefreshAllSkills();
        }
    }
}
