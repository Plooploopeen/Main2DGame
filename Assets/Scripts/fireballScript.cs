using UnityEngine;

public class fireballScript : SpellBase
{
    private Rigidbody2D rb;

    [SerializeField] float speed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        moveFireball();
    }

    void moveFireball()
    {
        if (isFacingRight)
        {
            Vector2 newVelocity = rb.linearVelocity;
            newVelocity.x += speed * Time.deltaTime;
            rb.linearVelocity = newVelocity;
        }
        else if (!isFacingRight)
        {
            Vector2 newVelocity = rb.linearVelocity;
            newVelocity.x = -speed * Time.deltaTime;
            rb.linearVelocity = newVelocity;
        }
    }
}
