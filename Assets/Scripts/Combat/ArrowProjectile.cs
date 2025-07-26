using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;

    private Vector2 direction;
    private EquipmentItem weapon;
    private float timer;

    public void Initialize(Vector2 dir, EquipmentItem weaponItem)
    {
        direction = dir.normalized;
        weapon = weaponItem;
        timer = lifetime;
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Enemy enemy = collider.GetComponent<Enemy>();
        if (enemy != null)
        {
            int skillTotal = PlayerStats.Instance.GetSkillTotal(SkillType.Bows);
            int hitRoll = DiceRoller.Roll(DiceType.D20) + skillTotal;

            InGameConsole.Instance.SendMessageToConsole($"Arrow hit roll: d20 + Bows({skillTotal}) = {hitRoll} vs {enemy.Defense}");

            if (hitRoll >= enemy.Defense)
            {
                int damageRoll = DiceRoller.Roll(weapon.weaponDice);
                int attrBonus = SkillAttributeMap.GetAttributeBonus(SkillType.Bows, PlayerStats.Instance.attributes);
                int totalDamage = damageRoll + attrBonus + weapon.flatBonusDamage;

                enemy.TakeDamage(totalDamage, direction);
                Destroy(gameObject); // Only destroy if a hit
            }
            else
            {
                InGameConsole.Instance.SendMessageToConsole("Missed! Arrow keeps flying...");
                // Do not destroy — arrow keeps flying
            }

            return;
        }

        // Hit a wall or other object
        Destroy(gameObject);
    }

}
