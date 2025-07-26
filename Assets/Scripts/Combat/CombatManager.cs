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

    private IPlayerClass currentClass;

    private void Start()
    {
        equipmentManager = PlayerEquipmentManager.Instance;
        playerStats = PlayerStats.Instance;
        console = InGameConsole.Instance;

        playerAnimator = gameObject.GetComponentInChildren<Animator>();
        playerController = gameObject.GetComponentInChildren<TopDownCharacterController>();

        if (equipmentManager == null) Debug.LogError("PlayerEquipmentManager not found!");
        if (playerStats == null) Debug.LogError("PlayerStats not found!");

        // placed here for testing purposes. Eventually a character creation screen will be
        // implemented and a set class method will handle setting the players class at start
        SetPlayerClass(new ArcherClass());
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

    public void SetPlayerClass(IPlayerClass playerClass)
    {
        currentClass = playerClass;
        Debug.Log($"Player class set to {playerClass}!");
    }

    public bool IsCurrentClassFighter()
    {
        return currentClass is FighterClass;
    }

    public void PerformAttack()
    {
        if (currentClass != null)
        {
            currentClass.PerformAttack(playerStats, equipmentManager, playerController);
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
        int hitRoll = DiceRoller.Roll(DiceType.D20) + skillTotal;

        console.SendMessageToConsole($"Fighter Hit Roll: d20 + Skill({skillTotal}) = {hitRoll} vs Enemy Defense: {enemy.Defense}");

        if (hitRoll >= enemy.Defense)
        {
            int diceRoll = DiceRoller.Roll(weapon.weaponDice);
            int attributeBonus = SkillAttributeMap.GetAttributeBonus(skill, playerStats.attributes);
            int totalDamage = diceRoll + attributeBonus + weapon.flatBonusDamage;

            console.SendMessageToConsole($"Fighter Damage: Dice({diceRoll}) + Attr({attributeBonus}) + Weapon({weapon.flatBonusDamage}) = {totalDamage}");

            Vector2 knockbackDir = playerController.GetFacingDirection();
            enemy.TakeDamage(totalDamage, knockbackDir);

            SkillUsageTracker.RegisterSkillUse(skill, totalDamage * 0.2f);
        }
        else
        {
            console.SendMessageToConsole("Fighter missed!");
        }
    }


}
