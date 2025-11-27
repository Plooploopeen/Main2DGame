using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class PlayerHealthScript : MonoBehaviour, IDamageable
{
    public float health = 100;
    [SerializeField] float maxHealth = 100;
    [SerializeField] float flashLength;
    [SerializeField] float knockbackForce;

    private SpriteRenderer SpriteRenderer;
    private Rigidbody2D rb;

    void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        
    }

    public void takeDamage(float damage, Transform attackerTransform)
    {
        

        health -= damage;

        Debug.Log(health);

        StartCoroutine(FlashRed());

        applyKnockback(transform.position - attackerTransform.position);

        if (health <= 0)
        {
            die();
        }
    }

    public void die()
    {
        Destroy(gameObject);
    }

    IEnumerator FlashRed()
    {
        Color original = SpriteRenderer.color;
        SpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashLength);
        SpriteRenderer.color = original;

    }

    void applyKnockback(Vector2 direction)
    {
        direction.Normalize();
        direction.y = 0;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
    }
}
