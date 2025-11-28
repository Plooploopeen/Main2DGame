using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class PlayerHealthScript : MonoBehaviour, IDamageable
{
    public float health = 100;
    public float maxHealth = 100;
    public bool isKnockedBack;
    [SerializeField] float flashLength;
    [SerializeField] float knockbackForce;
    [SerializeField] float knockbackLength;

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

        StartCoroutine(FlashRed());

        StartCoroutine(ApplyKnockback(transform.position - attackerTransform.position));

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

    IEnumerator ApplyKnockback(Vector2 direction)
    {
        isKnockedBack = true;
        direction.Normalize();
        direction.y = 1;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockbackLength);
        isKnockedBack = false;       
    }
}
