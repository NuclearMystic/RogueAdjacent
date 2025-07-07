using UnityEngine;

public class TopDownCharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Speed of the player when walking.")]
    public float walkSpeed = 2f;

    [Tooltip("Speed of the player when running.")]
    public float runSpeed = 4f;

    [Tooltip("The vertical force applied when the player jumps.")]
    public float jumpForce = 5f;

    [Header("State Variables")]
    [Tooltip("True if the player is running, false if walking.")]
    private bool isRunning = false;

    [Tooltip("True if the player is currently in a jump state.")]
    private bool isJumping = false;

    [Header("Components")]
    [Tooltip("The Rigidbody2D component used to move the player.")]
    private Rigidbody2D rb;

    [Tooltip("The Animator component used to control player animations.")]
    private Animator animator;

    // Constants for directions
    private const string Up = "Up";
    private const string Down = "Down";
    private const string Left = "Left";
    private const string Right = "Right";

    // Track facing direction (default facing down)
    private Vector2 lastDirection = Vector2.down;

    private void Start()
    {
        // Get necessary components
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Set default facing direction when game starts
        SetDirection(Down);
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        HandleRunWalkToggle();
    }

    private void HandleMovement()
    {
        // Get input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Determine speed (walk or run)
        float speed = isRunning ? runSpeed : walkSpeed;

        // Apply movement based on input
        Vector2 movement = new Vector2(moveX, moveY).normalized * speed;

        // Override the Rigidbody2D's velocity to apply movement, but ignore gravity
        rb.linearVelocity = movement;

        // Update the facing direction and animator parameters
        UpdateDirection(moveX, moveY);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    private void UpdateDirection(float moveX, float moveY)
    {
        // If there is any movement
        if (moveX != 0 || moveY != 0)
        {
            Vector2 currentDirection = new Vector2(moveX, moveY);

            // Only set the trigger if the player actually changes direction
            if (currentDirection != lastDirection)
            {
                lastDirection = currentDirection;  // Update the last direction

                if (Mathf.Abs(moveX) > Mathf.Abs(moveY))
                {
                    if (moveX > 0)
                        SetDirection(Right);
                    else
                        SetDirection(Left);
                }
                else
                {
                    if (moveY > 0)
                        SetDirection(Up);
                    else
                        SetDirection(Down);
                }
            }
        }
    }

    private void SetDirection(string direction)
    {
        // Reset all triggers before setting a new one
        animator.ResetTrigger("TurnUp");
        animator.ResetTrigger("TurnDown");
        animator.ResetTrigger("TurnLeft");
        animator.ResetTrigger("TurnRight");

        // Set the correct direction trigger
        animator.SetTrigger($"Turn{direction}");
    }

    private void HandleJump()
    {
        // Jump if allowed
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            isJumping = true;

            // Apply the jump force
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            // Determine jump direction based on lastDirection
            if (lastDirection == Vector2.up)
            {
                animator.SetTrigger("JumpUp");
            }
            else if (lastDirection == Vector2.down)
            {
                animator.SetTrigger("JumpDown");
            }
            else if (lastDirection == Vector2.left)
            {
                animator.SetTrigger("JumpLeft");
            }
            else if (lastDirection == Vector2.right)
            {
                animator.SetTrigger("JumpRight");
            }
        }
    }

    [ContextMenu("Reset Jump State")]
    public void ResetJump()
    {
        // Reset jumping state after landing
        isJumping = false;
    }

    private void HandleRunWalkToggle()
    {
        // Toggle between running and walking
        if (Input.GetButtonDown("Run/Walk Toggle"))
        {
            isRunning = !isRunning;

            // Update animator's isRunning parameter
            animator.SetBool("isRunning", isRunning);
        }
    }

    public Vector2 GetFacingDirection()
    {
        return lastDirection;
    }
}
