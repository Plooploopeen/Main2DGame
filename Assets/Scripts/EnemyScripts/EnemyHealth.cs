using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using TMPro;


public class EnemyHealth : MonoBehaviour, IDamageable
{
    private float health;
    private bool isFlashing = false;
    public bool isKnockedBack;
    [SerializeField] float maxHealth;
    [SerializeField] float knockbackForce;
    [SerializeField] float flashLength;
    [SerializeField] float knockbackLength;

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

    private void Update()
    {
        Debug.Log(health);
    }

    public void takeDamage(float damage, Transform attackerTransform)
    {
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
        if (GameObject.FindGameObjectWithTag("Sword") == null)
        {
            Destroy(gameObject);
        }
        else
        {
            GameObject swordObject = GameObject.FindGameObjectWithTag("Sword");
            swordScript swordScript = swordObject.GetComponent<swordScript>();

            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            PlayerSwordThrowingScript playerSwordThrowingScript = playerObject.GetComponent<PlayerSwordThrowingScript>();

            if (swordScript.enemyStuckInCollider != null && swordScript.playerSwordThrowingScript.swordInstance.transform.parent == gameObject.transform)
            {
                playerSwordThrowingScript.swordInstance.transform.SetParent(null);
                Destroy(gameObject);
                swordScript.GravityOn = true;
                swordScript.EnableAllCollision();
            }
        }
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

    IEnumerator ApplyKnockback(Vector2 direction)
    {
        isKnockedBack = true;
        direction.Normalize();
        direction.y = 1f;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockbackLength);
        isKnockedBack = false;
    }
}
