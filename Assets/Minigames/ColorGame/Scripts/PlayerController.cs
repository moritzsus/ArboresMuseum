using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 14f;

    [Header("Jump Timings")]
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.2f;

    [Header("Variable Jump")]
    public float lowJumpMultiplier = 2.5f;
    public float fallMultiplier = 2.0f;

    [Header("Wall Slide")]
    public float wallSlideSpeed = 2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator animator;
    private float horizontalInput;

    private float lastGroundedTime;
    private float lastJumpPressedTime;

    private bool isJumping;
    private bool isWallSliding;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        // Initialize timers to prevent unwanted jump at start
        lastJumpPressedTime = -1f;
        lastGroundedTime = -1f;
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // jump buffer timer
        if (Input.GetButtonDown("Jump"))
        {
            lastJumpPressedTime = Time.time;
        }

        HandleWallSlide();
        TryJump();

        // Flip sprite based on direction
        if (horizontalInput != 0)
            transform.localScale = new Vector3(Mathf.Sign(horizontalInput), 1, 1);
    }

    void FixedUpdate()
    {
        // horizontal movement
        Vector2 vel = rb.linearVelocity;
        vel.x = horizontalInput * moveSpeed;
        rb.linearVelocity = vel;

        bool grounded = IsGrounded();

        animator.SetBool("isWalking", horizontalInput != 0);
        animator.SetBool("isGrounded", grounded);
        animator.SetFloat("ySpeed", rb.linearVelocity.y);

        if (grounded) lastGroundedTime = Time.time;

        // variable jump height
        if (!grounded && !isWallSliding)
        {
            vel = rb.linearVelocity;

            if (vel.y < 0f)
            {
                float extra = (fallMultiplier - 1f) * Physics2D.gravity.y * rb.gravityScale * Time.fixedDeltaTime;
                vel.y += extra;
            }
            else if (vel.y > 0f && !Input.GetButton("Jump"))
            {
                float extra = (lowJumpMultiplier - 1f) * Physics2D.gravity.y * rb.gravityScale * Time.fixedDeltaTime;
                vel.y += extra;
            }

            rb.linearVelocity = vel;
        }

        if (grounded && isJumping) isJumping = false;
    }

    void TryJump()
    {
        // Jump buffering & coyote time logic
        if ((Time.time - lastJumpPressedTime) <= jumpBufferTime &&
            ((Time.time - lastGroundedTime) <= coyoteTime))
        {
            float jumpX = rb.linearVelocity.x;
            Vector2 vel = rb.linearVelocity;
            vel.x = jumpX;
            vel.y = jumpForce;

            rb.linearVelocity = vel;

            isJumping = true;
            isWallSliding = false;
            lastJumpPressedTime = -1f;
        }
    }

    void HandleWallSlide()
    {
        bool touchingWall = IsTouchingWall();
        bool movingIntoWall = horizontalInput != 0 && Mathf.Sign(horizontalInput) == Mathf.Sign(transform.localScale.x);

        if (!IsGrounded() && touchingWall && movingIntoWall)
        {
            isWallSliding = true;

            Vector2 vel = rb.linearVelocity;
            vel.y = Mathf.Max(vel.y, -wallSlideSpeed);

            rb.linearVelocity = vel;
        }
        else
        {
            isWallSliding = false;
        }
    }

    bool IsGrounded()
    {
        Bounds bounds = coll.bounds;
        Vector2 origin = new Vector2(bounds.center.x, bounds.min.y);
        Vector2 size = new Vector2(bounds.size.x * 0.9f, 0.1f);

        return Physics2D.OverlapBox(origin, size, 0f, groundLayer);
    }

    bool IsTouchingWall()
    {
        Bounds bounds = coll.bounds;
        Vector2 originLeft = new Vector2(bounds.min.x - 0.05f, bounds.center.y);
        Vector2 originRight = new Vector2(bounds.max.x + 0.05f, bounds.center.y);
        Vector2 size = new Vector2(0.1f, bounds.size.y * 0.9f);

        return Physics2D.OverlapBox(originLeft, size, 0f, groundLayer) ||
               Physics2D.OverlapBox(originRight, size, 0f, groundLayer);
    }
}
