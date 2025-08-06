using System;
using UnityEngine;

public class ExperienceEvents
{
    public event Action<int> onEnemyKilledXPGained;
    public event Action<SkillType, float> onWeaponSkillGained;

    public void EnemyKilledXPGained(int xpAmount)
    {
        onEnemyKilledXPGained?.Invoke(xpAmount);
    }

    public void WeaponSkillGained(SkillType skill, float xpAmount)
    {
        onWeaponSkillGained?.Invoke(skill, xpAmount);
    }
}
