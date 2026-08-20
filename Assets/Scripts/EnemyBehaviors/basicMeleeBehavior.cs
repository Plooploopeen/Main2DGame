using UnityEngine;

public class basicMeleeBehavior : EnemyBehaviorBase
{
    [SerializeField] float frontRayLength;
    [SerializeField] LayerMask frontRayLayers;

    GameObject weaponGameObject;
    BoxCollider2D weaponCollider;

    private EnemyData data;

    void Awake()
    {
        data = GetComponent<EnemyData>();

        weaponGameObject = transform.Find("WeaponHitbox").gameObject;
        weaponCollider = weaponGameObject.GetComponent<BoxCollider2D>();

        // find and disable weapon on start
        GameObject weapon = transform.Find("WeaponHitbox").gameObject;
        weapon.SetActive(false);
    }

    public override void Enter()
    {

    }

    public override void Execute()
    {
        data.animator.Play("Attack");
    }

    public override void Exit()
    {

    }

    public bool shouldAttack()
    {
        float absScale = Mathf.Abs(transform.localScale.x);
        float direction = Mathf.Sign(data.playerTransform.position.x - transform.position.x);

        RaycastHit2D frontRay = Physics2D.Raycast(transform.position, Vector2.right * data.faceRight, frontRayLength, frontRayLayers);

        Debug.DrawRay(transform.position, Vector2.right * direction * frontRayLength, Color.red);

        if (frontRay.collider != null && !data.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            return true;
        }
        return false;
    }

    public void enableHitbox()
    {
        weaponGameObject.SetActive(true);
        weaponCollider.enabled = true;

        HitBox hitBox = weaponGameObject.GetComponent<HitBox>();
        hitBox.SetHitStop(0.01f);
    }

    public void disableHitbox()
    {
        weaponGameObject.SetActive(false);
        weaponCollider.enabled = false;
    }
}
