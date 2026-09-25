using UnityEngine;

public class FollowAtDistanceBehavior : EnemyBehaviorBase
{
    [SerializeField] float speed;
    [SerializeField] float playerSeparation;
    private Vector2 lastMoveDir;
    //private float lastJumpTime;
    //[SerializeField] float jumpRayOffset;
    //[SerializeField] float jumpRayLength;
    //[SerializeField] float jumpForce;
    //[SerializeField] float jumpCooldown;
    //[SerializeField] LayerMask layerMask;

    private EnemyData data;

    void Awake()
    {
        data = GetComponent<EnemyData>();
    }
    public override void Enter()
    {

    }

    public override void Execute()
    {
        // check direction and dont move if knocked back
        Vector2 targetPosition = new Vector2(data.playerTransform.position.x - (playerSeparation * Mathf.Sign(data.playerTransform.position.x - transform.position.x)), data.playerTransform.position.y);

        float direction = Mathf.Sign(targetPosition.x - transform.position.x);
        float distance = Mathf.Abs(Vector2.Distance(data.playerTransform.position, transform.position));
        float horizontalDistance = data.playerTransform.position.x - transform.position.x;

        if (Mathf.Abs(targetPosition.x - transform.position.x) < 0.5f)
        {
            if (!data.isKnockedBack)
            {
                data.rb.linearVelocity = new Vector2(0f, data.rb.linearVelocity.y);
            }
        }
        else
        {
            data.rb.linearVelocity = new Vector2(speed * direction, data.rb.linearVelocity.y);
            lastMoveDir = data.rb.linearVelocity;
        }


        //// use ray casts to jump
        //Vector2 jumpRayPosition = (Vector2)transform.position + Vector2.down * jumpRayOffset;
        //RaycastHit2D jumpRay = Physics2D.Raycast(jumpRayPosition, Vector2.right * direction, jumpRayLength, layerMask);
        //Debug.DrawRay(jumpRayPosition, Vector2.right * direction * jumpRayLength, Color.green);

        //if (jumpRay.collider != null && data.isGrounded && Time.time >= lastJumpTime + jumpCooldown)
        //{
        //    data.rb.linearVelocity = new Vector2(data.rb.linearVelocity.x, 0);
        //    data.rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        //    lastJumpTime = Time.time;
        //}
    }

    public override void Exit()
    {

    }
}
