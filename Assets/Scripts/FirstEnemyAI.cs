using UnityEngine;

public class FirstEnemyAI : MonoBehaviour
{
    private NewMonoBehaviourScript enemyHealthScript;

    [SerializeField] LayerMask layerMask;
    private Rigidbody2D rb;

    [SerializeField] private float speed;
    private Transform playerTransform;
    [SerializeField] float jumpRayLength;
    private bool isGrounded;
    [SerializeField] float jumpForce;
    [SerializeField] float moveDistance;

    [SerializeField] float rayCastLength;
    [SerializeField] float rayShiftLeftAmount;
    [SerializeField] float rayShiftRightAmount;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyHealthScript = GetComponent<NewMonoBehaviourScript>();
    }
    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerObject.transform;
    }

    void Update()
    {
        // check isGrounded
        Vector2 leftRayPosition = (Vector2)transform.position + Vector2.left * rayShiftLeftAmount;
        Vector2 rightRayPosition = (Vector2)transform.position + Vector2.right * rayShiftRightAmount;

        int jumplayerMask = ~LayerMask.GetMask("Hurtbox");

        RaycastHit2D middleHit = Physics2D.Raycast(transform.position, Vector2.down, rayCastLength, jumplayerMask);
        RaycastHit2D leftHit = Physics2D.Raycast(leftRayPosition, Vector2.down, rayCastLength, jumplayerMask);
        RaycastHit2D rightHit = Physics2D.Raycast(rightRayPosition, Vector2.down, rayCastLength, jumplayerMask);

        Debug.DrawRay(transform.position, Vector2.down * rayCastLength, Color.orange);
        Debug.DrawRay(leftRayPosition, Vector2.down * rayCastLength, Color.red);
        Debug.DrawRay(rightRayPosition, Vector2.down * rayCastLength, Color.yellow);

        isGrounded = (middleHit.collider != null && middleHit.collider.CompareTag("Jumpable")) ||
                     (leftHit.collider != null && leftHit.collider.CompareTag("Jumpable")) ||
                     (rightHit.collider != null && rightHit.collider.CompareTag("Jumpable"));

        // check direction and dont move if knocked back
        float direction = Mathf.Sign(playerTransform.position.x - transform.position.x);
        float distance = Mathf.Abs(Vector2.Distance(playerTransform.position, transform.position));

        if (!enemyHealthScript.isKnockedBack && distance > moveDistance)
        {
            rb.linearVelocity = new Vector2(speed * direction, rb.linearVelocity.y);
        }

        // use ray casts to jump
        RaycastHit2D jumpRay = Physics2D.Raycast(transform.position, Vector2.right * direction, jumpRayLength, layerMask);
        Debug.DrawRay(transform.position, Vector2.right * direction * jumpRayLength, Color.green);

        if (jumpRay.collider != null && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
