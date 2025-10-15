using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public InputActionAsset InputActions;

    [SerializeField] PlayerInput playerInput;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float jumpForce;
    [SerializeField] float walkForce;
    [SerializeField] float rayCastLength;
    [SerializeField] float rayShiftAmount;
    [SerializeField] float fallMultipierSlow;
    [SerializeField] float fallMultipierFast;
    [SerializeField] float timerJumpLimit;
    [SerializeField] float timerCoyoteTimeLimit;
    [SerializeField] float timerJumpBufferLimit;

    private InputAction jumpAction;
    private InputAction attackAction;
    private InputAction sprintAction;
    private InputAction moveAction;
    private bool isGrounded;
    private bool canJump;
    private bool isJumping;
    private float timerJump = 0f;
    private bool isJumpCompleted;
    private float timerCoyoteTime;
    private float timerJumpBuffer;
    private bool justJumped;

    private Vector2 velocity;
    private Vector2 moveDirection;


    private void Awake()
    {
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

    private void Update()
    {

        moveDirection = moveAction.ReadValue<Vector2>();

        capVelocity();
        checkIsGrounded();
        jump();
        coyoteTime();
        jumpBuffer();

    }

    void FixedUpdate()
    {
        velocity = rb.linearVelocity;

        movePlayer();

        rb.linearVelocity = velocity;
    }

    void movePlayer()
    {
        float horizontal = moveDirection.x;
        velocity.x = horizontal * walkForce;

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
        Vector2 leftRayPosition = (Vector2)transform.position + Vector2.left * rayShiftAmount;
        Vector2 rightRayPosition = (Vector2)transform.position + Vector2.right * rayShiftAmount;

        RaycastHit2D middleHit = Physics2D.Raycast(transform.position, Vector2.down, rayCastLength, LayerMask.GetMask("Ground"));
        RaycastHit2D leftHit = Physics2D.Raycast(leftRayPosition, Vector2.down, rayCastLength, LayerMask.GetMask("Ground"));
        RaycastHit2D rightHit = Physics2D.Raycast(rightRayPosition, Vector2.down, rayCastLength, LayerMask.GetMask("Ground"));

        //Debug.DrawRay(transform.position, Vector2.down * rayCastLength, Color.orange);
        // Debug.DrawRay(leftRayPosition, Vector2.down * rayCastLength, Color.red);
        // Debug.DrawRay(rightRayPosition, Vector2.down * rayCastLength, Color.yellow);

        if (middleHit.collider != null || leftHit.collider != null || rightHit.collider != null)
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
            timerJump < timerJumpLimit && timerCoyoteTime < timerCoyoteTimeLimit)
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
            isJumping = true;
            rb.linearVelocity = Vector2.up * jumpForce;
            timerJump += Time.deltaTime;
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
            isJumping = false;
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
}