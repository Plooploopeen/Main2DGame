using JetBrains.Annotations;
using TMPro;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerScript : MonoBehaviour
{

    [Header("References")]
    [SerializeField] InputActionAsset InputActions;
    private PlayerScript playerScript;
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerAttackScript playerCombatScript;
    private PlayerHealthScript playerHealthScript;
    private SpriteRenderer spriteRenderer;

    [Header("Jump Settings")]
    [SerializeField] float jumpForce;
    [SerializeField] float fallMultipierSlow;
    [SerializeField] float fallMultipierFast;
    [SerializeField] float timerJumpLimit;

    private bool canJump;
    private bool isJumping;
    private float timerJump = 0f;
    private bool isJumpCompleted;
    private bool justJumped;

    [Header("Coyote Time")]
    [SerializeField] float timerCoyoteTimeLimit;

    private float timerCoyoteTime;

    [Header("Jump Buffer")]
    [SerializeField] float timerJumpBufferLimit;

    private float timerJumpBuffer;


    [Header("IsGrounded")]
    [SerializeField] float rayCastLength;
    [SerializeField] float rayShiftLeftAmount;
    [SerializeField] float rayShiftRightAmount;

    private bool isGrounded;
    public bool IsGrounded => isGrounded;

    [Header("Player movement and input")]
    [SerializeField] float moveSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float decelRate;
    private bool isSliding;
    private bool isMoving;
    private bool isFalling;
    public bool isMovingRight;
    private bool isMovingLeft;
    private bool isSprinting;

    private InputAction jumpAction;
    private InputAction attackAction;
    private InputAction sprintAction;
    private InputAction moveAction;

    [Header("Public references")]

    [Header("Player Animator")]


    [Header("Extras")]
    private Vector2 velocity;
    public Vector2 moveDirection;



    private void Awake()
    {
        playerScript = GetComponent<PlayerScript>();
        playerCombatScript = GetComponent<PlayerAttackScript>();
        playerHealthScript = GetComponent<PlayerHealthScript>();
        playerCombatScript = GetComponent<PlayerAttackScript>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        jumpAction = InputSystem.actions.FindAction("Jump");
        attackAction = InputSystem.actions.FindAction("Attack");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        moveAction = InputSystem.actions.FindAction("Move");
    }

    void Start()
    {
        InputActions.FindActionMap("UI").Disable();
        InputActions.FindActionMap("Gameplay").Enable();

        timerJumpBuffer = timerJumpBufferLimit;
    }

    private void OnEnable()
    {
        InputActions.FindActionMap("Gameplay").Enable();
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("Gameplay").Disable();
    }

    void Update()
    {
        moveDirection = moveAction.ReadValue<Vector2>();

        // Movement
        sprint();
        capVelocity();
        checkIsGrounded();
        coyoteTime();
        jumpBuffer();
        jump();

        // Animations
        updateAnimations();
    }

    void FixedUpdate()
    {
        if (isSliding)
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, decelRate * Time.fixedDeltaTime);
            animator.Play("Stop Sprinting Slide");

            if (Mathf.Abs(rb.linearVelocity.x) < 0.5f)
            {

                rb.linearVelocity = Vector2.zero;
                isSliding = false;
            }
        }
        else
        {
            velocity = rb.linearVelocity;
            movePlayer();
            rb.linearVelocity = velocity;
        }
    }

    void movePlayer()
    {
        float horizontal = moveDirection.x;

        // Check player movement input
        if (moveDirection.x > 0.4 && !playerHealthScript.isKnockedBack)
        {
            isMovingRight = true;
        }
        else
        {
            isMovingRight = false;
        }

        if (moveDirection.x < -0.4 && !playerHealthScript.isKnockedBack)
        {
            isMovingLeft = true;
        }
        else
        {
            isMovingLeft = false;
        }

        // Apply movement and face direction
        if (isMovingRight)
        {
            velocity.x = moveSpeed;

            spriteRenderer.flipX = false;
        }

        if (isMovingLeft)
        {
            velocity.x = -moveSpeed;

            spriteRenderer.flipX = true;
        }

        if (!isMovingLeft && !isMovingRight && !playerHealthScript.isKnockedBack)
            {
                velocity.x = 0;
            }
        
        // Make jumps fast and falling slow
        if (velocity.y < 0f)
        {
            rb.gravityScale = fallMultipierSlow;
        }
        else
        {
            rb.gravityScale = fallMultipierFast;
        }   
    }

    //Velocity cap

    void capVelocity()
    {
        if (velocity.y < -3)
        {
            velocity.y = -3;
        }
    }

    void checkIsGrounded()
    {
        Vector2 leftRayPosition = (Vector2)transform.position + Vector2.left * rayShiftLeftAmount;
        Vector2 rightRayPosition = (Vector2)transform.position + Vector2.right * rayShiftRightAmount;

        RaycastHit2D middleHit = Physics2D.Raycast(transform.position, Vector2.down, rayCastLength);
        RaycastHit2D leftHit = Physics2D.Raycast(leftRayPosition, Vector2.down, rayCastLength);
        RaycastHit2D rightHit = Physics2D.Raycast(rightRayPosition, Vector2.down, rayCastLength);

        Debug.DrawRay(transform.position, Vector2.down * rayCastLength, Color.orange);
        Debug.DrawRay(leftRayPosition, Vector2.down * rayCastLength, Color.red);
        Debug.DrawRay(rightRayPosition, Vector2.down * rayCastLength, Color.yellow);

        if (middleHit.collider != null && middleHit.collider.CompareTag("Jumpable") || 
            leftHit.collider != null && leftHit.collider.CompareTag("Jumpable") || 
            rightHit.collider != null && rightHit.collider.CompareTag("Jumpable"))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void jump()
    {
        // Check if able to jump
        if (isGrounded && !isJumpCompleted &&
            timerJump < timerJumpLimit && timerCoyoteTime < timerCoyoteTimeLimit && !playerCombatScript.isAttacking)
        {
            canJump = true;
        }
        else if (timerJump >= timerJumpLimit || (!isGrounded && timerCoyoteTime >= timerCoyoteTimeLimit && !isJumping))
        {
            canJump = false;
        }

        // Check if jumping
        if (canJump && jumpAction.IsPressed())
        {
            rb.linearVelocity = Vector2.up * jumpForce;
            timerJump += Time.deltaTime;
        }
        
        // Check is the player is jumping
        if (jumpAction.IsPressed() && (velocity.y > 0 || timerCoyoteTime < timerCoyoteTimeLimit) && !playerCombatScript.isAttacking)
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }

        //check is jump gets interupted
        if (jumpAction.WasReleasedThisFrame())
        {
            if (velocity.y > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                isJumpCompleted = true;
            }
            else
            {
                isJumpCompleted = true;
            }
        }

        //Check if reached max jump height
        if (timerJump >= timerJumpLimit)
        {
            isJumpCompleted = true;
        }



        // Reset bools if grounded
        if (isGrounded && !jumpAction.IsPressed() || isGrounded && timerJumpBuffer < timerJumpBufferLimit)
        {
            isJumpCompleted = false;
            timerJump = 0;
            timerCoyoteTime = 0;
        }

    }

    void coyoteTime()
    {
        if (!isGrounded && timerCoyoteTime < timerCoyoteTimeLimit && !isJumping && !isJumpCompleted)
        {
            timerCoyoteTime += Time.deltaTime;
        }
        else if (isJumpCompleted)
        {
            timerCoyoteTime = timerCoyoteTimeLimit;
        }
    }

    void jumpBuffer()
    {
        if (jumpAction.WasPressedThisFrame())
        {
            timerJumpBuffer = 0;
        }

        if (jumpAction.IsPressed() && !isJumping)
        {

            timerJumpBuffer += Time.deltaTime;
        }
        else
        {
            timerJumpBuffer = timerJumpBufferLimit;
        }

    }

    void sprint()
    {
        if (sprintAction.IsPressed())
        {
            moveSpeed = sprintSpeed;
            isSprinting = true;
        }
        else if (!sprintAction.IsPressed())
        {
            moveSpeed = walkSpeed;
            isSprinting = false;
        }
    }

    void updateAnimations()
    {
        float horizontal = moveDirection.x;

        if (Mathf.Abs(horizontal) > 0.4f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }


        if (!isGrounded && velocity.y < 0)
        {
            isFalling = true;
        }
        else
        {
            isFalling = false;
        }

        // Check if player just stopped moving while at running
        if (moveAction.WasReleasedThisFrame() && Mathf.Abs(velocity.x) > walkSpeed && isGrounded)
        {
            isSliding = true;
        }

        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isFalling", isFalling);
        animator.SetBool("isAttacking", playerCombatScript.isAttacking);
        animator.SetBool("isSprinting", isSprinting);
    }
    
    public void onIdleCompleted()
    {
        if (Random.Range(0, 40) == 1)
        {
            animator.Play("Sword Slip");
        }
    }

    public void onStopSprintingSlide()
    {
        animator.Play("Idle");
    }
}

