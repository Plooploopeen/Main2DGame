using UnityEngine;

public class FirstEnemyAI : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float speed;
    private Transform playerTransform;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerObject.transform;
    }

    void Update()
    {
        float direction = Mathf.Sign(playerTransform.position.x - transform.position.x);

        rb.linearVelocity = new Vector2(speed * direction, rb.linearVelocity.y);
    }
}
