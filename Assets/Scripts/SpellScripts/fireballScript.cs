using UnityEngine;

public class fireballScript : SpellBase
{
    private Rigidbody2D rb;

    [SerializeField] float speed;
    private bool castRight;
    [SerializeField] float maxDistance;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (playerScript.scale > 0)
        {
            castRight = true;
        }
        else if (playerScript.scale < 0)
        {
            castRight = false;
        }

    }

    private void Update()
    {
        moveFireball();

        if (Vector2.Distance(transform.position, playerScript.transform.position) > maxDistance)
        {
            Destroy(gameObject);
        }
    }

    void moveFireball()
    {
        if (castRight)
        {
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
