using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Enemy))]
public class EnemyAI : MonoBehaviour
{
    [Header("Detection Settings")]
    public float detectionRadius = 6f;
    public float attackRange = 1.5f;
    public float giveUpRange = 12f;

    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float wanderRadius = 3f;
    public float wanderInterval = 2f;

    [Header("Combat Settings")]
    public float attackCooldown = 1.5f;

    private Transform player;
    private Enemy enemy;
    private Vector2 spawnPoint;
    private Rigidbody2D rb;

    private float lastAttackTime;
    private Vector2 currentWanderTarget;

    private enum State { Idle, Chasing, Attacking, Returning }
    private State currentState;

    [Header("Sound Effects")]
    public AudioClip attackSFX;
    public AudioClip hitSFX;
    public AudioClip missSFX;
    public AudioClip stepsSFX;
    private AudioSource audioSource;

    [Header("Animation")]
    [Tooltip("Animator with DirX, DirY, IsMoving, Attack parameters")]
    [SerializeField] private Animator animator;
    private enum Cardinal { Up, Down, Left, Right }
    // default facing if idle on spawn
    private Vector2 lastNonZeroDir = Vector2.down; 

    [Header("Enemy Damage")]
    [Tooltip("Select type of dice for this enemy's damage.")]
    public DiceType damageDie;
    [Tooltip("This will be in addition to the hit roll of the enemy.")]
    public int hitModifier;
    [Tooltip("This will be in addition to any damage this enemy deals.")]
    public int damageModifier;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        enemy = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();
        spawnPoint = transform.position;
        currentState = State.Idle;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(WanderRoutine());
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Idle:
                if (distanceToPlayer <= detectionRadius)
                    currentState = State.Chasing;
                break;

            case State.Chasing:
                if (distanceToPlayer > giveUpRange)
                {
                    currentState = State.Returning;
                }
                else if (distanceToPlayer <= attackRange)
                {
                    currentState = State.Attacking;
                }
                else
                {
                    MoveToward(player.position);
                }
                break;

            case State.Attacking:
                if (distanceToPlayer > attackRange)
                {
                    currentState = State.Chasing;
                }
                else if (Time.time - lastAttackTime >= attackCooldown)
                {
                    AttackPlayer();
                    lastAttackTime = Time.time;
                }
                FaceTarget(player.position);
                break;

            case State.Returning:
                float distToSpawn = Vector2.Distance(transform.position, spawnPoint);
                if (distToSpawn <= 0.2f)
                {
                    currentState = State.Idle;
                }
                else
                {
                    MoveToward(spawnPoint);
                }
                break;
        }

        //  Always update animator here
        bool moving = rb.linearVelocity.sqrMagnitude > 0.0001f;
        if (moving)
            UpdateFacingFromVector(rb.linearVelocity.normalized);

        UpdateAnimator(moving ? rb.linearVelocity.normalized : lastNonZeroDir, moving);
    }


    private void MoveToward(Vector2 target)
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        rb.linearVelocity = dir * moveSpeed;

        UpdateFacingFromVector(dir);
        UpdateAnimator(dir, true);
    }

    private void FaceTarget(Vector2 target)
    {
        Vector2 dir = target - (Vector2)transform.position;

        // keep your scale flip for left/right if you want
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            transform.localScale = new Vector3(dir.x > 0 ? 1 : -1, 1, 1);

        // also update anim facing so attack uses the right directional clip
        Vector2 ndir = dir.sqrMagnitude > 0.0001f ? dir.normalized : lastNonZeroDir;
        UpdateFacingFromVector(ndir);
        UpdateAnimator(ndir, false); // not moving while in attack windup/loop
    }

    private void AttackPlayer()
    {
        // ensure attack uses current facing toward player
        FaceTarget(player.position);

        // kick the animation
        if (animator != null) animator.SetTrigger("Attack");

        // your existing SFX and hit logic
        SFXManager.Instance.PlaySFXFromSource(audioSource, attackSFX, 1, 1, 1);
        int hitRoll = DiceRoller.Roll(DiceType.D20) + hitModifier;
        int playerDefense = PlayerEquipmentManager.Instance.GetArmorBonus();

        InGameConsole.Instance.SendMessageToConsole("Enemy Hit Roll: d20 + " + hitModifier + " = " + hitRoll + " vs Player Defense: " + playerDefense);

        if (hitRoll >= playerDefense)
        {
            SFXManager.Instance.PlaySFXFromSource(audioSource, hitSFX, 1, 1, 1);
            int damage = DiceRoller.Roll(damageDie) + damageModifier;
            InGameConsole.Instance.SendMessageToConsole(enemy.enemyName + " hits you for " + damage + " damage!");
            PlayerVitals.instance.DamageHealth(damage);
        }
        else
        {
            SFXManager.Instance.PlaySFXFromSource(audioSource, missSFX, 1, 1, 1);
            InGameConsole.Instance.SendMessageToConsole(enemy.enemyName + " missed!");
        }
    }

    private IEnumerator WanderRoutine()
    {
        while (true)
        {
            if (currentState == State.Idle)
            {
                Vector2 randomOffset = Random.insideUnitCircle * wanderRadius;
                currentWanderTarget = spawnPoint + randomOffset;
                MoveToward(currentWanderTarget);
            }
            else
            {
                // when not explicitly moving via MoveToward, keep animator updated for idle
                Vector2 v = rb.linearVelocity;
                bool moving = v.sqrMagnitude > 0.0001f;
                if (moving) UpdateFacingFromVector(v.normalized);
                UpdateAnimator(moving ? v.normalized : lastNonZeroDir, moving);
            }
            yield return new WaitForSeconds(wanderInterval);
        }
    }

    private void UpdateFacingFromVector(Vector2 dir)
    {
        if (dir.sqrMagnitude < 0.0001f) return;
        // Snap to cardinal so your 4-dir clips look right
        Cardinal c = ToCardinal(dir);
        switch (c)
        {
            case Cardinal.Up: lastNonZeroDir = Vector2.up; break;
            case Cardinal.Down: lastNonZeroDir = Vector2.down; break;
            case Cardinal.Left: lastNonZeroDir = Vector2.left; break;
            case Cardinal.Right: lastNonZeroDir = Vector2.right; break;
        }
    }

    private void UpdateAnimator(Vector2 dir, bool isMoving)
    {
        if (animator == null) return;

        // For a 4-direction setup, feed exact cardinals to avoid diagonal blends
        Vector2 snapped = SnapToCardinal(dir);

        animator.SetBool("isMoving", isMoving);
        animator.SetFloat("DirX", snapped.x);
        animator.SetFloat("DirY", snapped.y);
    }

    private static Cardinal ToCardinal(Vector2 v)
    {
        if (Mathf.Abs(v.x) >= Mathf.Abs(v.y))
            return v.x >= 0f ? Cardinal.Right : Cardinal.Left;
        else
            return v.y >= 0f ? Cardinal.Up : Cardinal.Down;
    }

    private static Vector2 SnapToCardinal(Vector2 v)
    {
        if (Mathf.Abs(v.x) >= Mathf.Abs(v.y))
            return v.x >= 0f ? Vector2.right : Vector2.left;
        else
            return v.y >= 0f ? Vector2.up : Vector2.down;
    }
}
