using System;
using UnityEngine;

public class ExperienceEvents
{
    public event Action<int> onEnemyKilledXPGain;
    public event Action<SkillType, float> onWeaponSkillGained;

    public void EnemyKilledStrengthGained(int xpAmount)
    {
        onEnemyKilledXPGain?.Invoke(xpAmount);
    }

    public void WeaponSkillGained(SkillType skill, float xpAmount)
    {
        onWeaponSkillGained?.Invoke(skill, xpAmount);
    }
}
