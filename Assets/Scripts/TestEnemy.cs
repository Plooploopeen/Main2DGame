using JetBrains.Annotations;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Rendering;

public class TestEnemy : MonoBehaviour
{

    [Header("References")]
    [SerializeField] GameObject weaponGameObject;
    [SerializeField] BoxCollider2D weaponCollider;
    [SerializeField] Transform playerTransform;

    public HitBox hitBoxScript;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    //-------other---------//
    [SerializeField] float frontRayLength;
    [SerializeField] LayerMask frontRayLayers;

    private bool isFacingRight;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        hitBoxScript = weaponGameObject.GetComponent<HitBox>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        weaponGameObject.SetActive(false);
        weaponCollider.enabled = false;
    }

    void Update()
    {
        checkForPlayer();
        facePlayer();
    }

    void checkForPlayer()
    {
        float direction = Mathf.Sign(playerTransform.position.x - transform.position.x);

        RaycastHit2D frontRay = Physics2D.Raycast(transform.position, Vector2.right * direction, frontRayLength, frontRayLayers);

        Debug.DrawRay(transform.position, Vector2.right * frontRayLength, Color.red);

        if (frontRay.collider != null && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animator.Play("Attack");
        }


    }

    void facePlayer()
    {
        float direction = Mathf.Sign(playerTransform.position.x - transform.position.x);
        float absScale = Mathf.Abs(transform.localScale.x);

        if (direction > 0)
        {
            isFacingRight = true;
            transform.localScale = new Vector3(absScale, absScale, absScale);
        }
        else if (direction < 0)
        {
            isFacingRight = false;
            transform.localScale = new Vector3(-absScale, absScale, absScale);
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
