using UnityEngine;
using UnityEngine.Rendering;

public class TestEnemy : MonoBehaviour
{
    [SerializeField] float frontRayLength;
    [SerializeField] LayerMask frontRayLayers;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        RaycastHit2D frontRay = Physics2D.Raycast(transform.position, Vector2.right, frontRayLength, frontRayLayers);

        Debug.DrawRay(transform.position, Vector2.right * frontRayLength, Color.red);

        if (frontRay.collider != null && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animator.Play("Attack");
        }

    }
}
