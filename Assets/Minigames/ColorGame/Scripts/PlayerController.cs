using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 14f;

    [Header("Jump Timings")]
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.2f;

    [Header("Wall Slide")]
    public float wallSlideSpeed = 2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private float horizontalInput;

    private float lastGroundedTime;
    private float lastJumpPressedTime;

    private bool isJumping;
    private bool isWallSliding;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
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
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        // Update grounded time
        if (IsGrounded())
            lastGroundedTime = Time.time;
    }

    void TryJump()
    {
        // Jump buffering & coyote time logic
        if ((Time.time - lastJumpPressedTime) <= jumpBufferTime &&
            ((Time.time - lastGroundedTime) <= coyoteTime || isWallSliding))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isJumping = true;
            isWallSliding = false;
            lastJumpPressedTime = -1f; // reset buffer
        }
    }

    void HandleWallSlide()
    {
        bool touchingWall = IsTouchingWall();
        bool movingIntoWall = horizontalInput != 0 && Mathf.Sign(horizontalInput) == Mathf.Sign(transform.localScale.x);

        if (!IsGrounded() && touchingWall && movingIntoWall)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed);
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

    //#if UNITY_EDITOR
    //    void OnDrawGizmosSelected()
    //    {
    //        if (!coll) return;

    //        // Ground check box
    //        Bounds bounds = coll.bounds;
    //        Vector2 origin = new Vector2(bounds.center.x, bounds.min.y);
    //        Vector2 size = new Vector2(bounds.size.x * 0.9f, 0.1f);
    //        Gizmos.color = Color.green;
    //        Gizmos.DrawWireCube(origin, size);
    //    }
    //#endif


    //[SerializeField]
    //private float movementSpeed = 7.5f;
    //[SerializeField]
    //private float jumpForce = 23.0f;

    //private float coyoteTime = 0.2f;
    //private float coyoteTimer = 0f;
    //private float jumpCutMultiplier = 0.5f;

    //private float groundCheckDistance;
    //private bool jumpInputBuffer = false;
    //private bool isPlayerInAir = false;
    //private bool jumpCancelled = false;
    //private float jumpeTime;
    //private bool isFalling = false;
    //private float horizontalInput = 0;
    //private float lastY;

    //private Transform playerTransform;
    //private Rigidbody2D playerRigidbody;
    //private SpriteRenderer playerSr;
    //private LayerMask groundLayer;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    playerTransform = gameObject.GetComponent<Transform>();
    //    playerRigidbody = gameObject.GetComponent<Rigidbody2D>();
    //    playerSr = gameObject.GetComponent<SpriteRenderer>();
    //    groundCheckDistance = (playerSr.bounds.size.y / 2.0f) + 0.05f;
    //    groundLayer = LayerMask.GetMask("Ground");

    //    lastY = playerTransform.position.y;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    // if only FixedUpdate checks for KeyDown it may skip some inputs because it does not run every frame
    //    if (Input.GetKeyDown(KeyCode.Space) && !jumpInputBuffer && !isPlayerInAir)
    //    {
    //        jumpInputBuffer = true;
    //    }

    //    if (Input.GetKeyUp(KeyCode.Space) && playerRigidbody.linearVelocity.y > 0)
    //    {
    //        jumpCancelled = true;
    //    }
    //}

    //private void FixedUpdate()
    //{
    //    HandleMovement();

    //    lastY = playerTransform.position.y;
    //}

    //private void HandleMovement()
    //{
    //    horizontalInput = Input.GetAxisRaw("Horizontal");

    //    if (horizontalInput != 0)
    //    {
    //        playerTransform.position += Time.fixedDeltaTime * movementSpeed * new Vector3(horizontalInput, 0, 0);
    //        playerTransform.localScale = new Vector3(horizontalInput, 1, 1);
    //    }

    //    if (jumpCancelled)
    //    {
    //        playerRigidbody.linearVelocity = new Vector2(playerRigidbody.linearVelocity.x, playerRigidbody.linearVelocity.y * jumpCutMultiplier);
    //        jumpCancelled = false;
    //    }

    //    // ?? Bodencheck
    //    RaycastHit2D hit = Physics2D.Raycast(playerTransform.position, Vector2.down, groundCheckDistance, groundLayer);
    //    bool isGrounded = hit.collider != null;

    //    if (isGrounded)
    //    {
    //        coyoteTimer = coyoteTime;
    //        isPlayerInAir = false;
    //    }
    //    else
    //    {
    //        coyoteTimer -= Time.fixedDeltaTime;
    //    }

    //    // ?? Coyote Jump Logik
    //    if (jumpInputBuffer && coyoteTimer > 0f)
    //    {
    //        jumpInputBuffer = false;
    //        isPlayerInAir = true;
    //        jumpeTime = Time.time;
    //        playerRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    //    }

    //    // ?? Fallzustand bestimmen
    //    if (playerTransform.position.y < lastY)
    //    {
    //        if (!isFalling)
    //        {
    //            isFalling = true;
    //            isPlayerInAir = true;
    //        }
    //    }
    //    else
    //    {
    //        if (isFalling)
    //        {
    //            isFalling = false;
    //        }
    //    }
    //}
}
