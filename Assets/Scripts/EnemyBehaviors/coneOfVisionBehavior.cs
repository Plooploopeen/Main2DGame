using Unity.VisualScripting;
using UnityEngine;

public class coneOfVisionBehavior : MonoBehaviour, IEnemyBehavior
{
    [SerializeField] float detectionRange;
    [SerializeField] float detectionAngle;

    private EnemyData data;

    void Awake()
    {
        data = GetComponent<EnemyData>();
    }

    public void Enter()
    {

    }

    public void Execute()
    {
        float distance = Vector2.Distance(transform.position, data.playerTransform.position);

        if (distance > detectionRange)
        {
            return;
        }

        Vector2 directionToPlayer = (data.playerTransform.position - transform.position).normalized;
        Vector2 enemyForward = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        //Vector2 leftBoundary = Quaternion.Euler(0, 0, detectionAngle) * enemyForward;
        //Vector2 rightBoundary = Quaternion.Euler(0, 0, -detectionAngle) * enemyForward;
        //Debug.DrawRay(transform.position, enemyForward * detectionRange, Color.blue);
        //Debug.DrawRay(transform.position, leftBoundary * detectionRange, Color.green);
        //Debug.DrawRay(transform.position, rightBoundary * detectionRange, Color.green);

        float angle = Vector2.Angle(enemyForward, directionToPlayer);

        if (angle > detectionAngle)
        {
            return;
        }

        int layermask = ~LayerMask.GetMask("Hurtbox");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, detectionRange, layermask);

        if ((hit.collider != null && hit.collider.CompareTag("Player")))
        {
            Debug.Log("Seen player");
            data.hasSeenPlayer = true;
        }
    }

    public void Exit()
    {

    }
}
