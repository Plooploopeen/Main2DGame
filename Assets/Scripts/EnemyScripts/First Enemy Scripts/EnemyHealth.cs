using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using TMPro;


public class EnemyHealth : MonoBehaviour, IDamageable
{
    EnemyData data;

    private float health;
    private bool isFlashing = false;
    private bool hasLanded;
    [SerializeField] float maxHealth;
    [SerializeField] float knockbackForce;
    [SerializeField] float flashLength;
    [SerializeField] float knockbackLength;
    [SerializeField] float maxVelocity;

    private SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        data = GetComponentInParent<EnemyData>();

        spriteRenderer = transform.parent.GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        health = maxHealth;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (data.isKnockedBack && !data.isGrounded)
        {
            hasLanded = false;
        }

        if (data.isGrounded && !hasLanded && data.isKnockedBack)
        {
            data.isKnockedBack = false;
            hasLanded = true;
        }
    }

    public virtual void takeDamage(float damage, Transform attackerTransform)
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
        Destroy(transform.root.gameObject);
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
        data.isKnockedBack = true;
        data.rb.linearVelocity = Vector2.zero;
        direction.y = 5f;
        direction.Normalize();
        data.rb.linearVelocity = direction * knockbackForce;

        // Cap velocity so combos don't send enemy flying
        data.rb.linearVelocity = new Vector2(
            Mathf.Clamp(data.rb.linearVelocity.x, -maxVelocity, maxVelocity),
            Mathf.Clamp(data.rb.linearVelocity.y, -maxVelocity, maxVelocity)
        );

        hasLanded = true;
    }
}