using UnityEngine;

public class WizardClass : IPlayerClass
{
    public void PerformAttack(PlayerStats stats, PlayerEquipmentManager equipment, TopDownCharacterController controller)
    {
        Debug.Log("Attack performed!");
        EquipmentItem weapon = PlayerEquipmentManager.Instance.GetCurrentHeldWeapon();
        if (weapon == null)
        {
            InGameConsole.Instance.SendMessageToConsole("No bow equipped!");
            return;
        }

        PlayerVitals.instance.DrainStamina(weapon.stamDrain - stats.GetSkillTotal(SkillType.Bows) * 0.1f);

        Vector2 direction = controller.GetFacingDirection().normalized;
        Vector2 spawnOffset = direction * 1.0f; // increase from 0.5f to give it more space
        Vector2 spawnPoint = (Vector2)controller.transform.position + spawnOffset;

        GameObject arrowPrefab = Resources.Load<GameObject>("Arrow");
        if (arrowPrefab != null)
        {
            GameObject arrowInstance = GameObject.Instantiate(arrowPrefab, spawnPoint, Quaternion.identity);
            ArrowProjectile arrowScript = arrowInstance.GetComponent<ArrowProjectile>();
            if (arrowScript != null)
            {
                arrowScript.Initialize(direction, weapon);
            }
        }
        else
        {
            Debug.LogError("Arrow prefab not found in Resources!");
        }
    }
}
