using JetBrains.Annotations;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Rendering;

public class TestEnemy : MonoBehaviour
{
    [SerializeField] float frontRayLength;
    [SerializeField] LayerMask frontRayLayers;

    public HitBox hitBoxScript;

    private Animator animator;

    [SerializeField] GameObject weaponGameObject;
    [SerializeField] BoxCollider2D weaponCollider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        hitBoxScript = weaponGameObject.GetComponent<HitBox>();
    }
    void Start()
    {
        weaponGameObject.SetActive(false);
    }

    void Update()
    {
        RaycastHit2D frontRay = Physics2D.Raycast(transform.position, Vector2.right, frontRayLength, frontRayLayers);

        Debug.DrawRay(transform.position, Vector2.right * frontRayLength, Color.red);

        if (frontRay.collider != null && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animator.Play("Attack");
        }

        

        //Debug.Log(hitBoxScript.hitCount);

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
