using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class PlayerVitals : MonoBehaviour
{

    #region Singleton
    public static PlayerVitals instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion 

    [Header("Sliders")]
    [Tooltip("Slider from the health bar.")]
    [SerializeField] private Slider healthSlider;
    [Tooltip("Slider from the stamina bar.")]
    [SerializeField] private Slider staminaSlider;
    [Tooltip("Slider from the magic bar.")]
    [SerializeField] private Slider magicSlider;

    [Header("Conditions")]
    [Tooltip("Is the player currently in combat?")]
    public bool inCombat = false;
    private bool isHealing = false;
    private bool isRestoringStamina = false;
    private bool isRestoringMagic = false;

    private Coroutine healingCoroutine;
    private Coroutine staminaRegenCoroutine;
    private Coroutine magicRegenCoroutine;

    // health
    [SerializeField] private float currentHealth;
    private float MaxHealth(){
        float maxHealth = 90 + PlayerStats.Instance.GetAttributeTotal(AttributeType.STR) * 10;
        return maxHealth;
    }

    // stamina
    public float currentStamina { get; private set; }
    private float MaxStamina()
    {
        float maxStamina = 90 + PlayerStats.Instance.GetAttributeTotal(AttributeType.DEX) * 10;
        return maxStamina;
    }

    // magic
    private float currentMagic;
    private float MaxMagic()
    {
        float maxMagic = 90 + PlayerStats.Instance.GetAttributeTotal(AttributeType.INT) * 10;
        return maxMagic;
    }

private CombatManager combatManager;
    private SkillType armorSkill;

    void Start()
    {
        // initialize vitals with starting max values based off attributes
        InitializeVitals();

        combatManager = GetComponentInParent<CombatManager>();
        SetArmorSkill();
        // initialize sliders with starting current health
        RefreshBarsUI();
    }

    public void InitializeVitals()
    {
        healthSlider.maxValue = MaxHealth();
        staminaSlider.maxValue = MaxStamina();
        magicSlider.maxValue = MaxMagic();
        currentHealth = MaxHealth();
        currentStamina = MaxStamina();
        currentMagic = MaxMagic();
    }

    private void Update()
    {
        // Clamping values
        currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth());
        currentStamina = Mathf.Clamp(currentStamina, 0, MaxStamina());
        currentMagic = Mathf.Clamp(currentMagic, 0, MaxMagic());

        if (currentHealth == 0)
        {
            DeathHandling();
        }

        // Start passive healing if health is not full, player is not dead, and not in combat
        if (currentHealth < MaxHealth() && currentHealth > 0 && !inCombat && !isHealing)
        {
            healingCoroutine = StartCoroutine(PassiveHealing());
        }

        // Stop healing if player is in combat, at full health, or dead
        if (currentHealth == MaxHealth() || inCombat || currentHealth == 0)
        {
            StopHealing();
        }

        // Start stamina and magic regeneration if not full
        if (currentStamina < MaxStamina() && !isRestoringStamina)
        {
            staminaRegenCoroutine = StartCoroutine(PassiveStaminaRegen());
        }

        if (currentMagic < MaxMagic() && !isRestoringMagic)
        {
            magicRegenCoroutine = StartCoroutine(PassiveMagicRegen());
        }
    }

    private void SetArmorSkill()
    {
        PlayerClass playerClass = combatManager.playerClass;

        if (combatManager == null)
        {
            Debug.LogWarning("Armor Type Not Set");
            return;
        }
        else
        {
            switch (playerClass)
            {
                case PlayerClass.Archer: armorSkill = SkillType.LightArmor; break;
                case PlayerClass.Fighter: armorSkill = SkillType.HeavyArmor; break;
                case PlayerClass.Wizard: armorSkill = SkillType.MageArmor; break;
                default: armorSkill = SkillType.LightArmor; break;
            }
        }
    }
    public void RefreshBarsUI()
    {
        // refresh slider values so they match current health values
        healthSlider.value = currentHealth;
        staminaSlider.value = currentStamina;
        magicSlider.value = currentMagic;
        // update max values for each slider
        healthSlider.maxValue = MaxHealth();
        staminaSlider.maxValue = MaxStamina();
        magicSlider.maxValue = MaxMagic();
    }

    #region Health Logic
    // call this method with a damage value to damage player health
    public void DamageHealth(float damage)
    {
        currentHealth -= damage;

        PlayerStats.Instance.AddSkillXP(armorSkill, damage * .2f);

        // Stop healing if damaged
        StopHealing();

        // Start healing again after 5 seconds
        if (!inCombat && currentHealth > 0 && currentHealth < MaxHealth())
        {
            healingCoroutine = StartCoroutine(PassiveHealing());
        }

        RefreshBarsUI();
        InGameConsole.Instance.SendMessageToConsole("Player health damaged " + damage);
    }

    // call this method with a healing value to restore player health
    public void RestoreHealth(float healing)
    {
        currentHealth += healing;
        RefreshBarsUI();
        InGameConsole.Instance.SendMessageToConsole("Player health restored " + healing);
    }

    private IEnumerator PassiveHealing()
    {
        isHealing = true;

        // Wait for 5 seconds before starting healing
        yield return new WaitForSeconds(5);

        // Continue healing while the player is not at max health and not in combat
        while (currentHealth < MaxHealth() && !inCombat)
        {
            currentHealth += Time.deltaTime;
            RefreshBarsUI();
            yield return null;
        }

        isHealing = false;
    }

    private void StopHealing()
    {
        if (healingCoroutine != null)
        {
            StopCoroutine(healingCoroutine);
            healingCoroutine = null;
            isHealing = false;
        }
    }
    #endregion

    #region Stamina Logic
    // call this with the amount of stamina to drain from player
    public void DrainStamina(float stamina)
    {
        StopStaminaRegen(); 
        StartCoroutine(DrainPlayerStamina(stamina));
        RefreshBarsUI();
    }

    // makes stamina drain over time
    private IEnumerator DrainPlayerStamina(float drainAmount)
    {
        float drainedStamina = drainAmount;
        while (drainedStamina > 0)
        {
            currentStamina -= drainAmount * Time.deltaTime;
            drainedStamina -= drainAmount * Time.deltaTime;
            RefreshBarsUI();
            yield return null;
        }

        // Start stamina regeneration if it's not already running
        if (staminaRegenCoroutine == null)
        {
            staminaRegenCoroutine = StartCoroutine(PassiveStaminaRegen());
        }
    }

    // call this to restore player stamina
    public void RestoreStamina(float stamina)
    {
        StopStaminaRegen();
        StartCoroutine(RestorePlayerStamina(stamina));
        RefreshBarsUI();
        InGameConsole.Instance.SendMessageToConsole("Player stamina restored " + stamina);
    }

    // stamina restores over time as well
    private IEnumerator RestorePlayerStamina(float restoreAmount)
    {
        float restoredStamina = restoreAmount;
        while (restoredStamina > 0)
        {
            currentStamina += restoreAmount * Time.deltaTime;
            restoredStamina -= restoreAmount * Time.deltaTime;
            RefreshBarsUI();
            yield return null;
        }

        // Restart regeneration after manual restore
        staminaRegenCoroutine = StartCoroutine(PassiveStaminaRegen());
    }

    private IEnumerator PassiveStaminaRegen()
    {
        isRestoringStamina = true;
        yield return new WaitForSeconds(1);

        while (currentStamina < MaxStamina())
        {
            // Base regen rate at skill 0
            float baseRegen = 2.0f;

            // Add diminishing bonus from skill
            float regen = baseRegen + Mathf.Log(5f, 2f);

            currentStamina += Time.deltaTime * regen;
            RefreshBarsUI();
            yield return null;
        }


        isRestoringStamina = false;
        staminaRegenCoroutine = null;
    }

    private void StopStaminaRegen()
    {
        if (staminaRegenCoroutine != null)
        {
            StopCoroutine(staminaRegenCoroutine);
            staminaRegenCoroutine = null;
            isRestoringStamina = false;
        }
    }
    #endregion

    #region Magic Logic
    public void UseMagic(float magic)
    {
        StopMagicRegen(); 
        currentMagic -= magic;
        RefreshBarsUI();

        magicRegenCoroutine = StartCoroutine(PassiveMagicRegen());
    }

    public void ReplenishMagic(float magic)
    {
        StopMagicRegen();
        StartCoroutine(RestorePlayerMagic(magic));
        RefreshBarsUI();
        InGameConsole.Instance.SendMessageToConsole("Player magic replenished " + magic);
    }

    // magic restores over time instead of instantly
    private IEnumerator RestorePlayerMagic(float restoreAmount)
    {
        float restoredMagic = restoreAmount;
        while (restoredMagic > 0)
        {
            currentMagic += restoreAmount * Time.deltaTime;
            restoredMagic -= restoreAmount * Time.deltaTime;
            RefreshBarsUI();
            yield return null;
        }

        magicRegenCoroutine = StartCoroutine(PassiveMagicRegen());
    }

    private IEnumerator PassiveMagicRegen()
    {
        isRestoringMagic = true;
        yield return new WaitForSeconds(5);

        while (currentMagic < MaxMagic())
        {
            currentMagic += Time.deltaTime;
            RefreshBarsUI();
            yield return null;
        }

        isRestoringMagic = false;
    }

    private void StopMagicRegen()
    {
        if (magicRegenCoroutine != null)
        {
            StopCoroutine(magicRegenCoroutine);
            magicRegenCoroutine = null;
            isRestoringMagic = false;
        }
    }
    #endregion


    public void RestoreAllVitals()
    {
        currentHealth = MaxHealth();
        currentStamina = MaxStamina();
        currentMagic = MaxMagic();
        RefreshBarsUI();
    }

    private void DeathHandling()
    {
        GameManager.Instance.WarpBackToOriginalPosition();
        InGameConsole.Instance.SendMessageToConsole("Player has died. R.I.P.");
    }
}
