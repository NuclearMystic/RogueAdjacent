using UnityEngine;

public class AttackEventRelay : MonoBehaviour
{
    private CombatManager combatManager;

    private void Awake()
    {
        combatManager = FindAnyObjectByType<CombatManager>();
    }

    public void PerformAttack()
    {
        Debug.Log(PlayerEquipmentManager.Instance.HasWeaponEquipped());
        if (PlayerEquipmentManager.Instance.HasWeaponEquipped())
        {
            Debug.Log("attacking.");
            combatManager?.PerformAttack();
        }
        else
        {
            InGameConsole.Instance.SendMessageToConsole("No weapon to attack with...");
            Debug.Log("No weapon equipped.");
        }

    }
}