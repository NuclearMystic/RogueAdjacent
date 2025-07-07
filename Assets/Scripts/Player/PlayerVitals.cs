using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVitals : MonoBehaviour
{

    #region Singleton
    public static PlayerVitals instance;

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

    [Header("Modifiers")]
    [Tooltip("Placeholder modifier representing attribute that will affect max health.")]
    public float healthAttribute = 1.0f;
    [Tooltip("Placeholder modifier representing attribute that will affect max stamina.")]
    public float staminaAttribute = 1.0f;
    [Tooltip("Placeholder modifier representing attribute that will affect max magic.")]
    public float magicAttribute = 1.0f;

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
    private float currentHealth;
    private float maxHealth = 100;

    // stamina
    public float currentStamina { get; private set; }
    private float maxStamina = 100;

    // magic
    private float currentMagic;
    private float maxMagic = 100;

    void Start()
    {
        // initialize vitals with starting max values based off attributes
        InitializeVitals();

        // initialize sliders with starting current health
        RefreshBarsUI();
    }

    public void InitializeVitals()
    {
        SetMaxValues();
        healthSlider.maxValue = maxHealth;
        staminaSlider.maxValue = maxStamina;
        magicSlider.maxValue = maxMagic;
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        currentMagic = maxMagic;
    }

    private void Update()
    {
        // Clamping values
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        currentMagic = Mathf.Clamp(currentMagic, 0, maxMagic);

        if (currentHealth == 0)
        {
            DeathHandling();
        }

        // Start passive healing if health is not full, player is not dead, and not in combat
        if (currentHealth < maxHealth && currentHealth > 0 && !inCombat && !isHealing)
        {
            healingCoroutine = StartCoroutine(PassiveHealing());
        }

        // Stop healing if player is in combat, at full health, or dead
        if (currentHealth == maxHealth || inCombat || currentHealth == 0)
        {
            StopHealing();
        }

        // Start stamina and magic regeneration if not full
        if (currentStamina < maxStamina && !isRestoringStamina)
        {
            staminaRegenCoroutine = StartCoroutine(PassiveStaminaRegen());
        }

        if (currentMagic < maxMagic && !isRestoringMagic)
        {
            magicRegenCoroutine = StartCoroutine(PassiveMagicRegen());
        }
    }

    public void RefreshBarsUI()
    {
        // refresh slider values so they match current health values
        healthSlider.value = currentHealth;
        staminaSlider.value = currentStamina;
        magicSlider.value = currentMagic;
    }

    #region Health Logic
    // call this method with a damage value to damage player health
    public void DamageHealth(float damage)
    {
        currentHealth -= damage;

        // Stop healing if damaged
        StopHealing();

        // Start healing again after 5 seconds
        if (!inCombat && currentHealth > 0 && currentHealth < maxHealth)
        {
            healingCoroutine = StartCoroutine(PassiveHealing());
        }

        RefreshBarsUI();
        Debug.Log("Player health damaged " + damage);
    }

    // call this method with a healing value to restore player health
    public void RestoreHealth(float healing)
    {
        currentHealth += healing;
        RefreshBarsUI();
        Debug.Log("Player health restored " + healing);
    }

    private IEnumerator PassiveHealing()
    {
        isHealing = true;

        // Wait for 5 seconds before starting healing
        yield return new WaitForSeconds(5);

        // Continue healing while the player is not at max health and not in combat
        while (currentHealth < maxHealth && !inCombat)
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
        Debug.Log("Player stamina restored " + stamina);
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

        while (currentStamina < maxStamina)
        {
            float skill = PlayerStats.Instance.GetSkillTotal(SkillType.Athletics);
            // Base regen rate at skill 0
            float baseRegen = 2.0f;

            // Add diminishing bonus from skill
            float regen = baseRegen + Mathf.Log(skill + 1f, 2f);

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
        Debug.Log("Player used magic " + magic);

        magicRegenCoroutine = StartCoroutine(PassiveMagicRegen());
    }

    public void ReplenishMagic(float magic)
    {
        StopMagicRegen();
        StartCoroutine(RestorePlayerMagic(magic));
        RefreshBarsUI();
        Debug.Log("Player magic replenished " + magic);
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

        while (currentMagic < maxMagic)
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

    public void SetMaxValues()
    {
        maxHealth = 100 * healthAttribute;
        maxStamina = 100 * staminaAttribute;
        maxMagic = 100 * magicAttribute;
        RefreshBarsUI();
    }

    public void RestoreAllVitals()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        currentMagic = maxMagic;
        RefreshBarsUI();
    }

    private void DeathHandling()
    {
        Debug.Log("Player has died. R.I.P.");
    }
}
