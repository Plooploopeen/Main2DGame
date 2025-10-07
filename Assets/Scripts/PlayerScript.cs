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

    private InputAction jumpAction;
    private InputAction attackAction;
    private InputAction sprintAction;
    private InputAction moveAction;
    private bool isGrounded;
    private bool canJump;

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

        if (isGrounded)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }

        if (jumpAction.WasPressedThisFrame() && canJump)
        {
            jump();
        }
    }

    void FixedUpdate()
    {
        velocity = rb.linearVelocity;

        movePlayer();

        rb.linearVelocity = velocity;
    }

    void jump()
    {
        rb.linearVelocity = Vector2.up * jumpForce;
    }

    void movePlayer()
    {
        float horizontal = moveDirection.x;
        velocity.x = horizontal * walkForce;
    }

}
