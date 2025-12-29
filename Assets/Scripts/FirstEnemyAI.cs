//using System.Numerics;
using UnityEngine;

public class FirstEnemyAI : MonoBehaviour
{
    private NewMonoBehaviourScript enemyHealthScript;

    [SerializeField] LayerMask layerMask;
    private Rigidbody2D rb;
    private Animator animator;

    [SerializeField] float detectionRange;
    [SerializeField] float detectionAngle;

    [SerializeField] GameObject weaponGameObject;
    [SerializeField] BoxCollider2D weaponCollider;

    private HitBox hitBoxScript;

    private SpriteRenderer spriteRenderer;

    [SerializeField] private float speed;
    private Transform playerTransform;
    [SerializeField] float jumpRayLength;
    private bool isGrounded;
    [SerializeField] float jumpForce;
    [SerializeField] float moveDistance;
    [SerializeField] float frontRayLength;
    [SerializeField] LayerMask frontRayLayers;
    private bool hasSeenPlayer;
    private bool isFacingRight;
    private float patrolTime;
    private float faceRight;

    [SerializeField] float rayCastLength;
    [SerializeField] float rayShiftLeftAmount;
    [SerializeField] float rayShiftRightAmount;
    [SerializeField] float waitTimerLimit;
    [SerializeField] float moveTimerLimit;

    private enum PatrolState { moveLeft, moveRight, standStill };
    private PatrolState patrolState = PatrolState.moveRight;
    private PatrolState nextState;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyHealthScript = GetComponent<NewMonoBehaviourScript>();
        animator = GetComponent<Animator>();
        hitBoxScript = weaponGameObject.GetComponent<HitBox>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerObject.transform;
        weaponGameObject.SetActive(false);
        weaponCollider.enabled = false;
        hitBoxScript = GetComponentInChildren<HitBox>();
    }

    void Update()
    {
        Debug.Log(hasSeenPlayer);

        checkIsGrounded();
        checkDirection();

        if (enemyHealthScript.isKnockedBack)
        {
            return;
        }

        if (hasSeenPlayer)
        {
            chasePlayer();
            checkAttack();
        }
        else
        {
            patrol();
            checkForPlayer();
        }
        // Debug visualization
        Vector2 enemyForward = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        float drawDistance = detectionRange;

        // Draw forward direction
        Debug.DrawRay(transform.position, enemyForward * drawDistance, Color.blue);

        // Draw cone boundaries
        Vector2 leftBoundary = Quaternion.Euler(0, 0, detectionAngle) * enemyForward;
        Vector2 rightBoundary = Quaternion.Euler(0, 0, -detectionAngle) * enemyForward;

        Color coneColor = hasSeenPlayer ? Color.red : Color.green;
        Debug.DrawRay(transform.position, leftBoundary * drawDistance, coneColor);
        Debug.DrawRay(transform.position, rightBoundary * drawDistance, coneColor);

        // Draw line to player
        if (playerTransform != null)
        {
            Debug.DrawLine(transform.position, playerTransform.position, Color.yellow);
        }
    }

    void checkIsGrounded()
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
    }

    void chasePlayer()
    {
        // check direction and dont move if knocked back
        float direction = Mathf.Sign(playerTransform.position.x - transform.position.x);
        float distance = Mathf.Abs(Vector2.Distance(playerTransform.position, transform.position));

        if (distance > moveDistance)
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

    void patrol()
    {
        patrolTime += Time.deltaTime;

        if (patrolState == PatrolState.moveRight)
        {
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);

            if (patrolTime > moveTimerLimit)
            {
                patrolTime = 0;
                patrolState = PatrolState.standStill;
                nextState = PatrolState.moveLeft;
            }
        }
        else if (patrolState == PatrolState.moveLeft)
        {
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);

            if (patrolTime > moveTimerLimit)
            {
                patrolTime = 0;
                patrolState = PatrolState.standStill;
                nextState = PatrolState.moveRight;
            }
        }
        else if (patrolState == PatrolState.standStill)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

            if (patrolTime > waitTimerLimit)
            {
                patrolTime = 0;

                if (nextState == PatrolState.moveRight)
                {
                    patrolState = PatrolState.moveRight;
                }
                else
                {
                    patrolState = PatrolState.moveLeft;
                }
            }
        }
    }

    void checkForPlayer()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance > detectionRange)
        {
            return;
        }

        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
        Vector2 enemyForward = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        float angle = Vector2.Angle(enemyForward, directionToPlayer);

        if (angle > detectionAngle)
        {
            return;
        }

        int layermask = ~LayerMask.GetMask("Hurtbox");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, detectionRange, layermask);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            hasSeenPlayer = true;
        }
    }

    void checkDirection()
    {
        float absScale = Mathf.Abs(transform.localScale.x);

        if (hasSeenPlayer)
        {
            float direction = Mathf.Sign(playerTransform.position.x - transform.position.x);
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
        else
        {
            if (rb.linearVelocity.x > 0 && isGrounded)
            {
                transform.localScale = new Vector3(absScale, absScale, absScale);
                faceRight = 1;
            }
            else if (isGrounded)
            {
                transform.localScale = new Vector3(-absScale, absScale, absScale);
                faceRight = -1;
            }
        }
    }

    void checkAttack()
    {
        float absScale = Mathf.Abs(transform.localScale.x);
        float direction = Mathf.Sign(playerTransform.position.x - transform.position.x);

        RaycastHit2D frontRay = Physics2D.Raycast(transform.position, Vector2.right * faceRight, frontRayLength, frontRayLayers);

        Debug.DrawRay(transform.position, Vector2.right * direction * frontRayLength, Color.red);

        if (frontRay.collider != null && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animator.Play("Attack");
        }
    }

    public void enableHitbox()
    {
        weaponGameObject.SetActive(true);
        weaponCollider.enabled = true;
    }

    public void disableHitbox()
    {
        weaponGameObject.SetActive(false);
        weaponCollider.enabled = false;
    }
}
