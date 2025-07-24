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
            // UIController.Instance.ShowLevelUpPopup($"{skill} +1!");

            // Only give attribute XP if the skill actually leveled up
            AttributeType governingAttribute = SkillAttributeMap.GetPrimaryAttribute(skill);
            if (governingAttribute != AttributeType.None)
            {
                float attributeXP = xpAmount * AttributeXPPercent;
                bool attributeLeveledUp = PlayerStats.Instance.AddAttributeXP(governingAttribute, attributeXP);

                if (attributeLeveledUp)
                {
                    // UIController.Instance.ShowLevelUpPopup($"{governingAttribute} +1!");
                }
            }
        }

        // Refresh skill list when menu opens and closes to show most recent updates to skills
        //if (UIController.Instance.inventoryAnimator.GetBool("inventoryOpen"))
        //{
        //    UIController.Instance.skillMenuUI.RefreshAllSkills();
        //}
    }
}
