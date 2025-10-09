using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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

    void Update()
    {
        //Velocity cap

        if (velocity.y < -3)
        {
            velocity.y = -3;
        }

        moveDirection = moveAction.ReadValue<Vector2>();


        // Check isGrounded

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



        //Jump

        if (isGrounded && !jumpAction.IsPressed() && timerJumpBuffer >= timerJumpBufferLimit)
        {
            isJumpCompleted = false;
            canJump = true;
            timerJump = 0;
        }

        if (timerJump < timerJumpLimit && canJump && jumpAction.IsPressed() && !isJumpCompleted && timerCoyoteTime < timerCoyoteTimeLimit)
        {
            isJumping = true;
            rb.linearVelocity = Vector2.up * jumpForce;
            timerJump += Time.deltaTime;
        }
        else
        {
            isJumping = false;
        }

        if (timerJump >= timerJumpLimit && timerJumpBuffer >= timerJumpBufferLimit)
        {
            canJump = false;
            isJumpCompleted = true;
        }

        if (jumpAction.WasReleasedThisFrame() && velocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            isJumpCompleted = true;
        }

        if (!isGrounded && !isJumping && timerCoyoteTime >= timerCoyoteTimeLimit)
        {
            isJumpCompleted = true;
        }
        
            
        




        //Coyote time

        if (!isGrounded && timerCoyoteTime < timerCoyoteTimeLimit && !jumpAction.IsPressed())
        {
            timerCoyoteTime += Time.deltaTime;
        }

        if (isGrounded)
        {
            timerCoyoteTime = 0;
        }




        //Jump buffering

        if (jumpAction.WasPressedThisFrame())
        {
            timerJumpBuffer = 0;
        }
        else if (jumpAction.WasReleasedThisFrame())
        {
            timerJumpBuffer = timerJumpBufferLimit;
        }

        if (jumpAction.IsPressed() && timerJumpBuffer < timerJumpBufferLimit && !isJumping)
        {
            timerJumpBuffer += Time.deltaTime;
        }

        if (timerJumpBuffer < timerJumpBufferLimit && jumpAction.IsPressed() && isGrounded)
        {
            isJumpCompleted = false;
            canJump = true;
        }



            Debug.Log(canJump);

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

}
