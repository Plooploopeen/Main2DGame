    using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public Transform playerTransform { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator animator { get; private set; }
    public bool isGrounded { get; private set; }
    public float faceRight { get; private set; }

    public bool hasSeenPlayer { get; set; }

    [SerializeField] float rayCastLength;
    [SerializeField] float rayShiftLeftAmount;
    [SerializeField] float rayShiftRightAmount;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        checkIsGrounded();
        checkDirection();
    }

    void checkIsGrounded()
    {
        // check isGrounded
        Vector2 leftRayPosition = (Vector2)transform.position + Vector2.left * rayShiftLeftAmount;
        Vector2 rightRayPosition = (Vector2)transform.position + Vector2.right * rayShiftRightAmount;

        int jumplayerMask = LayerMask.GetMask("Ground");

        RaycastHit2D middleHit = Physics2D.Raycast(transform.position, Vector2.down, rayCastLength, jumplayerMask);
        RaycastHit2D leftHit = Physics2D.Raycast(leftRayPosition, Vector2.down, rayCastLength, jumplayerMask);
        RaycastHit2D rightHit = Physics2D.Raycast(rightRayPosition, Vector2.down, rayCastLength, jumplayerMask);

        Debug.DrawRay(transform.position, Vector2.down * rayCastLength, Color.orange);
        Debug.DrawRay(leftRayPosition, Vector2.down * rayCastLength, Color.red);
        Debug.DrawRay(rightRayPosition, Vector2.down * rayCastLength, Color.yellow);

        isGrounded = (middleHit.collider != null && middleHit.collider.CompareTag("Jumpable")) ||
                     (leftHit.collider != null && leftHit.collider.CompareTag("Jumpable")) ||
                     (rightHit.collider != null && rightHit.collider.CompareTag("Jumpable"));
    }

    void checkDirection()
    {
        float absScale = Mathf.Abs(transform.localScale.x);

        if (hasSeenPlayer)
        {
            float horizontalDistance = Mathf.Abs(playerTransform.position.x - transform.position.x);
            float direction = Mathf.Sign(playerTransform.position.x - transform.position.x);

            if (horizontalDistance > 0.566f)
            {
                if (direction > 0)
                {
                    transform.localScale = new Vector3(absScale, absScale, absScale);
                    faceRight = 1;
                }
                else
                {
                    transform.localScale = new Vector3(-absScale, absScale, absScale);
                    faceRight = -1;
                }
            }
        }
        else
        {
            if (rb.linearVelocity.x > 0.283f) // Removed isGrounded becuase of problems
            {
                transform.localScale = new Vector3(absScale, absScale, absScale);
                faceRight = 1;
            }
            else if (rb.linearVelocity.x < -0.283f) // Removed isGrounded becuase of problems
            {
                transform.localScale = new Vector3(-absScale, absScale, absScale);
                faceRight = -1;
            }
        }
    }
}
