using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using TMPro;


public class NewMonoBehaviourScript : MonoBehaviour, IDamageable
{
    private float health;
    private bool isFlashing = false;
    [SerializeField] float maxHealth;
    [SerializeField] float knockbackForce;
    [SerializeField] float flashLength;

    private Transform playerTransform;

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

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerObject.transform;
    }

    public void takeDamage(float damage, Transform attackerTransform)
    {
        health -= damage;

        if (!isFlashing) StartCoroutine(FlashRed());

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
        isFlashing = true;
        Color original = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashLength);
        spriteRenderer.color = original;
        isFlashing = false;

    }

    void applyKnockback(Vector2 direction)
    {
        direction.Normalize();
        direction.y = 1f;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
    }

}
