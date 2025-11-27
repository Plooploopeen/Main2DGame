using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;


public class NewMonoBehaviourScript : MonoBehaviour, IDamageable
{
    private float health;
    [SerializeField] float maxHealth;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        health = maxHealth;
    }

    public void takeDamage(float damage)
    {
        health -= damage;

        StartCoroutine(FlashRed());

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
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = original;

    }
}
