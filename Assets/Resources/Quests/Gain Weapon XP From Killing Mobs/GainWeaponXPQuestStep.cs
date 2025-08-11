using UnityEngine;

public class GainWeaponXPQuestStep : QuestStep
{
    private SkillType weaponSkill = SkillType.Axes;
    private float skillToGain = 100;
    private float skillGained = 0;

    private void Start()
    {
        stepDescription = "Gain " + skillToGain + " XP with Axes";
        stepName = "Axe Mastery";
    }

    private void OnEnable()
    {
        GameEventsManager.instance.experienceEvents.onWeaponSkillGained += WeaponSkillGained;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.experienceEvents.onWeaponSkillGained -= WeaponSkillGained;
    }

    private void WeaponSkillGained (SkillType skill, float gain)
    {
        if (skill == weaponSkill) { 
            skillGained += gain;
            InGameConsole.Instance.SendMessageToConsole($"Axe XP Gained. Quest Progress: {skillGained}/{skillToGain}");
            UpdateStepDescription();
        }

        if (skillGained >= skillToGain)
        {
            FinishQuestStep();
        }
    }

    private void UpdateStepDescription()
    {
        stepDescription = $"Gain {skillToGain} XP with Axes ({skillGained}/{skillToGain})";
    }
}
