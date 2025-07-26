using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    private CombatManager combatManager;
    private HashSet<Enemy> hitEnemies = new();

    private void OnEnable()
    {
        hitEnemies.Clear();
    }

    private void Awake()
    {
        combatManager = FindAnyObjectByType<CombatManager>();
        if (combatManager == null)
        {
            Debug.LogError("CombatManager not found in parent!");
        }

        // By default, disable collider
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null && !hitEnemies.Contains(enemy))
        {
            if (combatManager.IsCurrentClassFighter()) 
            {
                hitEnemies.Add(enemy);
                combatManager.Attack(enemy);
            }
        }
    }

}
