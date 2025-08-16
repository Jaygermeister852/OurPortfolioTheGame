using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField, Range(0f, 1f)] private float airControlPercent = 0.5f;
    [SerializeField] private float groundCheckRadius = 0.2f;

    [Header("States")]
    [SerializeField] private bool jumpQueued;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isMoving;
    [SerializeField] private bool isFrozen = false;

    [Header("References")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private PlayerSFXRelay sfxRelay;


    // Cache
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    private int idleLoopCount;


    // ------------------- LifeCycle -------------------


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    void OnEnable()
    {
        if (GameManager.Instance.SplashIsShowing)
        {
            SetFrozen(true);
        }
    }


    private void FixedUpdate()
    {
        CheckGrounded();

        if (isFrozen)
        {
            return;
        }

        Move();
        HandleJump();
    }





    // ------------------- Movement -------------------

    private void CheckGrounded()
    {
        bool touchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Only grounded if not moving upwards
        if (touchingGround && rb.linearVelocity.y <= 0.1f)
            isGrounded = true;
        else
            isGrounded = false;

        animator.SetBool("IsGrounded", isGrounded);
    }

    private void Move()
    {
        float currentSpeed = isGrounded ? moveSpeed : moveSpeed * airControlPercent;
        rb.linearVelocity = new Vector2(moveInput.x * currentSpeed, rb.linearVelocity.y);

        // Flip sprite direction
        if (moveInput.x > 0.01f)
            spriteRenderer.flipX = false;  // facing right
        else if (moveInput.x < -0.01f)
            spriteRenderer.flipX = true;   // facing left


        isMoving = rb.linearVelocity.x != 0;
        animator.SetBool("IsMoving", isMoving);
    }

    private void HandleJump()
    {
        if (jumpQueued && isGrounded)
        {
            sfxRelay.PlayJump();  //Plays jump sound

            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            jumpQueued = false;
        }
    }


    public void SetFrozen(bool frozen)
    {
        // Disable movement logic however you handle it
        isFrozen = frozen;

        // Disables moving animation
        animator.SetBool("IsMoving", false);

        // Optional: Zero velocity
        rb.linearVelocity = Vector2.zero;

        if (frozen)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; // lock X and rotation
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; // back to normal gameplay (only lock rotation)
        }
    }


    // ------------------- Splash screen movement lock -------------------

    private void HandleSplashStart()
    {
        SetFrozen(true);
    }

    private void HandleSplashEnd()
    {
        SetFrozen(false);
    }



    // ------------------- Input System -------------------

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            jumpQueued = true;
        }

        if (context.canceled)
        {
            jumpQueued = false;
        }
    }




    // Optional: visualize ground check in Scene view
    private void OnDrawGizmosSelected()
    {
        if (groundCheck)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }


    // ------------------- Idle Animation -------------------


    public void OnIdleLoop() // called by the Idle clip's animation event
    {
        idleLoopCount++;

        if (idleLoopCount >= 5)
        {
            ResetIdleLoopCount();
            animator.SetTrigger("IsDancing");
        }


    }

    public void OnDancingLoop()
    {
        idleLoopCount++;

        if (idleLoopCount >= 3)
        {
            ResetIdleLoopCount();
            animator.SetTrigger("IsSleeping");
        }
    }



    public void ResetIdleLoopCount()
    {
        idleLoopCount = 0;
    }
}