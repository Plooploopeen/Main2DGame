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



    [SerializeField] float frontRayLength;
    [SerializeField] LayerMask frontRayLayers;

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

        RaycastHit2D frontRay = Physics2D.Raycast(transform.position, Vector2.right, frontRayLength, frontRayLayers);

        Debug.DrawRay(transform.position, Vector2.right * frontRayLength, Color.red);

        if (frontRay.collider != null && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animator.Play("Attack");
        }


    }

    void facePlayer()
    {
        float direction = Mathf.Sign(playerTransform.position.x - transform.position.x);

        if (direction > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (direction < 0)
        {
            spriteRenderer.flipX = true;
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
