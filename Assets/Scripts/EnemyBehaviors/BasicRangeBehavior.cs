using UnityEngine;

public class basicRangeBehavior : EnemyBehaviorBase
{
    private EnemyData data;

    [SerializeField] GameObject axePrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float throwForceY;

    void Awake()
    {
        data = GetComponent<EnemyData>();


    }

    public override void Enter()
    {
        Debug.Log("Throw");

        float direction = Mathf.Sign(data.playerTransform.position.x - transform.position.x);
        GameObject axe = Instantiate(axePrefab, spawnPoint.position, Quaternion.identity);
        Rigidbody2D axeRb = axe.GetComponent<Rigidbody2D>();

        float gravity = Mathf.Abs(Physics2D.gravity.y) * axeRb.gravityScale;
        float horizontalDistance = Mathf.Abs(data.playerTransform.position.x - transform.position.x);
        float timeOfFlight = (2f * throwForceY) / gravity;
        float throwForceX = horizontalDistance / timeOfFlight;

        axeRb.linearVelocity = new Vector2(throwForceX * direction, throwForceY);



    }

    public override void Execute()
    {
        Debug.Log("Error");
    }

    public override void Exit()
    {

    }
}

