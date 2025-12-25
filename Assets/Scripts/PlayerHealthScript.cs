using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using Unity.VisualScripting;

public class PlayerHealthScript : MonoBehaviour, IDamageable
{
    private playerDefenceScript playerDefenceScript;

    private Color original;
    public float health = 100;
    public float maxHealth = 100;
    public bool isKnockedBack;
    private bool isFlashing = false;
    [SerializeField] float flashLength;
    [SerializeField] float knockbackForce;
    [SerializeField] float knockbackLength;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    void Awake()
    {
        playerDefenceScript = GetComponent<playerDefenceScript>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        health = maxHealth;
        original = spriteRenderer.color;
    }

    void Update()
    {
        
    }

    public void takeDamage(float damage, Transform attackerTransform)
    {
        if (playerDefenceScript.isParrying)
        {
            playerDefenceScript.onParrySuccess();
            return;
        }

        health -= damage;

        if (!isFlashing) StartCoroutine(FlashRed());

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
        isFlashing = true;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashLength);
        spriteRenderer.color = original;
        isFlashing = false;
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
