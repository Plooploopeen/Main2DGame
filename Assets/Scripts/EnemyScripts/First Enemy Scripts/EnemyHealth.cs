using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using TMPro;


public class EnemyHealth : MonoBehaviour, IDamageable
{
    EnemyData data;
    FirstEnemyAI firstEnemyAIScript;

    private float health;
    private bool isFlashing = false;
    public bool isKnockedBack;
    private bool hasLanded;
    [SerializeField] float maxHealth;
    [SerializeField] float knockbackForce;
    [SerializeField] float flashLength;
    [SerializeField] float knockbackLength;

    private SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        data = GetComponent<EnemyData>();

        firstEnemyAIScript = GetComponent<FirstEnemyAI>();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        health = maxHealth;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (isKnockedBack && !data.isGrounded)
        {
            hasLanded = false;
        }

        if (data.isGrounded && !hasLanded && isKnockedBack)
        {
            isKnockedBack = false;
            hasLanded = true;
        }
    }

    public void takeDamage(float damage, Transform attackerTransform)
    {
        health -= damage;

        if (!isFlashing) StartCoroutine(FlashRed());

        // this makes the enemy notice me if I damage it. I might have to change this later if I add an outside source of damage, like fall damage or
        // enemy friendly fire
        data.hasSeenPlayer = true;

        applyKnockback(transform.position - attackerTransform.position);

        if (health <= 0)
        {
            die();
        }
    }

    public void die()
    {
        if (GameObject.FindGameObjectWithTag("Sword") == null)
        {
            
        }
        else
        {
            GameObject swordObject = GameObject.FindGameObjectWithTag("Sword");
            swordScript swordScript = swordObject.GetComponent<swordScript>();

            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            PlayerSwordThrowingScript playerSwordThrowingScript = playerObject.GetComponent<PlayerSwordThrowingScript>();

            if (swordScript.enemyStuckInCollider != null && swordScript.enemyStuckInCollider == GetComponent<Collider2D>())
            {
                Debug.Log("sword varified as stuck");
                swordObject.transform.SetParent(null);
                swordScript.enemyStuckInCollider = null;
                swordScript.GravityOn = true;
                swordScript.EnableAllCollision();
            }
        }
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
        isKnockedBack = true;
        data.rb.linearVelocity = Vector2.zero;
        direction.y = 5f;
        direction.Normalize();
        data.rb.linearVelocity = direction * knockbackForce;
        hasLanded = true;
    }
}