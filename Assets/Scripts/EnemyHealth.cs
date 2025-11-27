using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;


public class NewMonoBehaviourScript : MonoBehaviour, IDamageable
{
    private float health;
    [SerializeField] float maxHealth;
    [SerializeField] float knockbackForce;
    [SerializeField] float flashLength;

    [SerializeField] Transform playerTransform;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        health = maxHealth;
    }

    public void takeDamage(float damage, Transform attackerTransform)
    {
        health -= damage;

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
        Color original = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashLength);
        spriteRenderer.color = original;

    }

    void applyKnockback(Vector2 direction)
    {
        direction.Normalize();
        direction.y = 1f;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
    }

}
