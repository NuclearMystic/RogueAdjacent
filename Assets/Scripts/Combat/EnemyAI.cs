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

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        enemy = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();
        spawnPoint = transform.position;
        currentState = State.Idle;
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
    }

    private void MoveToward(Vector2 target)
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        rb.linearVelocity = dir * moveSpeed;
    }

    private void FaceTarget(Vector2 target)
    {
        Vector2 dir = target - (Vector2)transform.position;
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            transform.localScale = new Vector3(dir.x > 0 ? 1 : -1, 1, 1);
    }

    private void AttackPlayer()
    {
        int hitRoll = DiceRoller.Roll(DiceType.D20) + 3; // +3 = enemy skill modifier (temp)
        int playerDefense = 12; // replace with actual player defense logic

        if (hitRoll >= playerDefense)
        {
            int damage = DiceRoller.Roll(DiceType.D6); // example: 1D6 damage
            InGameConsole.Instance.SendMessageToConsole($"{enemy.enemyName} hits you for {damage} damage!");
            PlayerVitals.instance.DamageHealth(damage);
            // TODO: Apply damage to player
        }
        else
        {
            InGameConsole.Instance.SendMessageToConsole($"{enemy.enemyName} missed!");
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
            yield return new WaitForSeconds(wanderInterval);
        }
    }
}
