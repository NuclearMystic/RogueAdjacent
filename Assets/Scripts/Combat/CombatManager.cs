using UnityEditor;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    // controls
    public KeyCode attackButton;
    public KeyCode defenseButton;

    private PlayerEquipmentManager equipmentManager;
    private PlayerStats playerStats;
    private InGameConsole console;

    private Animator playerAnimator;
    private TopDownCharacterController playerController;

    private void Start()
    {
        equipmentManager = PlayerEquipmentManager.Instance;
        playerStats = PlayerStats.Instance;
        console = InGameConsole.Instance;

        playerAnimator = gameObject.GetComponentInChildren<Animator>();
        playerController = gameObject.GetComponentInChildren<TopDownCharacterController>();

        if (equipmentManager == null) Debug.LogError("PlayerEquipmentManager not found!");
        if (playerStats == null) Debug.LogError("PlayerStats not found!");
    }

    private void Update()
    {
        if (Input.GetKeyDown(attackButton))
        {
            if (PlayerVitals.instance.currentStamina > 0)
            {
                playerAnimator.SetTrigger("Attack");
            }
        }
    }

    public void Attack(Enemy enemy)
    {
        EquipmentItem weapon = equipmentManager.GetCurrentHeldWeapon();

        if (weapon == null)
        {
            console.SendMessageToConsole("No weapon equipped!");
            return;
        }

        SkillType skill = weapon.weaponSkill;

        int skillTotal = playerStats.GetSkillTotal(skill);

        // Use DiceRoller for hit roll (simulate D20)
        int hitRoll = DiceRoller.Roll(DiceType.D20) + skillTotal;
        PlayerVitals.instance.DrainStamina(weapon.stamDrain - skillTotal * 0.1f);

        console.SendMessageToConsole($"Hit Roll: d20 + Skill({skillTotal}) = {hitRoll} vs Enemy Defense: {enemy.Defense}");

        if (hitRoll >= enemy.Defense)
        {
            // Damage calculation
            int diceRoll = DiceRoller.Roll(weapon.weaponDice);
            int attributeBonus = SkillAttributeMap.GetAttributeBonus(skill, playerStats.attributes);
            int totalDamage = diceRoll + attributeBonus + weapon.flatBonusDamage;

            console.SendMessageToConsole($"Hit! Damage: Dice({diceRoll}) + Attr({attributeBonus}) + Weapon({weapon.flatBonusDamage}) = {totalDamage}");

            Vector2 knockbackDir = playerController.GetFacingDirection();
            enemy.TakeDamage(totalDamage, knockbackDir);

            // Register skill use for XP gain
            SkillUsageTracker.RegisterSkillUse(skill, totalDamage * 0.2f); // Example: give 20% of damage as XP
        }
        else
        {
            console.SendMessageToConsole("Missed the attack!");
        }
    }
}
