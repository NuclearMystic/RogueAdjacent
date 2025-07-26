using UnityEngine;

public class FighterClass : IPlayerClass
{
    public void PerformAttack(PlayerStats stats, PlayerEquipmentManager equipment, TopDownCharacterController controller)
    {
        EquipmentItem weapon = equipment.GetCurrentHeldWeapon();
        if (weapon == null)
        {
            InGameConsole.Instance.SendMessageToConsole("No weapon equipped!");
            return;
        }

        float staminaCost = weapon.stamDrain - stats.GetSkillTotal(weapon.weaponSkill) * 0.1f;
        PlayerVitals.instance.DrainStamina(staminaCost);
    }
}
