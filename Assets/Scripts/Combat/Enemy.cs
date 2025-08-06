using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Info")]
    public string enemyName = "Goblin";
    public int maxHealth = 20;
    public int defense = 10;
    public int xpOnDeath = 10;

    private int currentHealth;

    [Header("Knockback Settings")]
    [SerializeField] private float knockbackForce = 5f;
    private Rigidbody2D rb;

    [Header("Flash Settings")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashDuration = 0.05f;
    [SerializeField] private int flashCount = 2;

    private bool isOnHitCooldown = false;
    private LootTable lootTable;

    private void Start()
    {
        lootTable = FindAnyObjectByType<LootTable>();
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
    }

    public int Defense => defense;

    public void TakeDamage(int amount, Vector2 knockbackDirection)
    {
        if (isOnHitCooldown) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        InGameConsole.Instance.SendMessageToConsole($"{enemyName} takes {amount} damage! HP: {currentHealth}/{maxHealth}");

        StartCoroutine(HitCooldown());
        ApplyKnockback(knockbackDirection);
        StartCoroutine(FlashSprite());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void ApplyKnockback(Vector2 direction)
    {
        if (rb == null) return;
        Debug.Log("tried to knockback)");
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction.normalized * knockbackForce, ForceMode2D.Impulse);
    }

    private IEnumerator FlashSprite()
    {
        if (spriteRenderer == null) yield break;

        Color originalColor = spriteRenderer.color;

        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }
    }

    private IEnumerator HitCooldown(float duration = 0.5f)
    {
        isOnHitCooldown = true;
        yield return new WaitForSeconds(duration);
        isOnHitCooldown = false;
    }

    private void Die()
    {
        InGameConsole.Instance.SendMessageToConsole($"{enemyName} has been slain!");
        GameEventsManager.instance.enemyEvents.EnemyKilled(enemyName);
        GameEventsManager.instance.experienceEvents.EnemyKilledStrengthGained(xpOnDeath);

        SkillType skill = PlayerEquipmentManager.Instance.GetCurrentHeldWeapon().weaponSkill;
        PlayerStats.Instance.AddSkillXP(skill, xpOnDeath);
        DropLoot();
        // TODO: Grant XP to player, drop loot, trigger animation, etc.
        Destroy(gameObject);
    }

    private void DropLoot()
    {
        if (lootTable == null) return;

        InventoryItem item = lootTable.GetRandomLootItem();
        if (item == null) return;

        Vector2 dropOffset = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
        Vector2 dropPosition = (Vector2)transform.position + dropOffset;

        Instantiate(item.itemPrefab, dropPosition, Quaternion.identity);
    }

    public float GetHealthPercent()
    {
        return (float)currentHealth / maxHealth;
    }

    public void SetStats(string name, int hp, int def, int xpReward)
    {
        enemyName = name;
        maxHealth = hp;
        currentHealth = hp;
        defense = def;
        xpOnDeath = xpReward;
    }
}
