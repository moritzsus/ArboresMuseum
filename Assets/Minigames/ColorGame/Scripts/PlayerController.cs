using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 7.5f;
    [SerializeField]
    private float jumpForce = 23.0f;

    private float coyoteTime = 0.2f;
    private float coyoteTimer = 0f;
    private float jumpCutMultiplier = 0.5f;

    private float groundCheckDistance;
    private bool jumpInputBuffer = false;
    private bool isPlayerInAir = false;
    private bool jumpCancelled = false;
    private float jumpeTime;
    private bool isFalling = false;
    private float horizontalInput = 0;
    private float lastY;

    private Transform playerTransform;
    private Rigidbody2D playerRigidbody;
    private SpriteRenderer playerSr;
    private LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = gameObject.GetComponent<Transform>();
        playerRigidbody = gameObject.GetComponent<Rigidbody2D>();
        playerSr = gameObject.GetComponent<SpriteRenderer>();
        groundCheckDistance = (playerSr.bounds.size.y / 2.0f) + 0.05f;
        groundLayer = LayerMask.GetMask("Ground");

        lastY = playerTransform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        // if only FixedUpdate checks for KeyDown it may skip some inputs because it does not run every frame
        if (Input.GetKeyDown(KeyCode.Space) && !jumpInputBuffer && !isPlayerInAir)
        {
            jumpInputBuffer = true;
        }

        if (Input.GetKeyUp(KeyCode.Space) && playerRigidbody.linearVelocity.y > 0)
        {
            jumpCancelled = true;
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();

        lastY = playerTransform.position.y;
    }

    private void HandleMovement()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput != 0)
        {
            playerTransform.position += Time.fixedDeltaTime * movementSpeed * new Vector3(horizontalInput, 0, 0);
            playerTransform.localScale = new Vector3(horizontalInput, 1, 1);
        }

        if (jumpCancelled)
        {
            playerRigidbody.linearVelocity = new Vector2(playerRigidbody.linearVelocity.x, playerRigidbody.linearVelocity.y * jumpCutMultiplier);
            jumpCancelled = false;
        }

        // ?? Bodencheck
        RaycastHit2D hit = Physics2D.Raycast(playerTransform.position, Vector2.down, groundCheckDistance, groundLayer);
        bool isGrounded = hit.collider != null;

        if (isGrounded)
        {
            coyoteTimer = coyoteTime;
            isPlayerInAir = false;
        }
        else
        {
            coyoteTimer -= Time.fixedDeltaTime;
        }

        // ?? Coyote Jump Logik
        if (jumpInputBuffer && coyoteTimer > 0f)
        {
            jumpInputBuffer = false;
            isPlayerInAir = true;
            jumpeTime = Time.time;
            playerRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // ?? Fallzustand bestimmen
        if (playerTransform.position.y < lastY)
        {
            if (!isFalling)
            {
                isFalling = true;
                isPlayerInAir = true;
            }
        }
        else
        {
            if (isFalling)
            {
                isFalling = false;
            }
        }
    }
}
