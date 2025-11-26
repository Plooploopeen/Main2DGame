using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class PlayerHealthScript : MonoBehaviour, IDamageable
{
    public float health = 100;
    [SerializeField] float maxHealth = 100;

    private SpriteRenderer SpriteRenderer;

    void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        
    }

    public void takeDamage(float damage)
    {
        health -= damage;

        Debug.Log(health);

        //StartCoroutine(FlashRed());

        if (health <= 0)
        {
            die();
        }
    }

    public void die()
    {
        Destroy(gameObject);
    }

    //IEnumerator FlashRed()
    //{
    //    Color original = SpriteRenderer.color;
    //    SpriteRenderer.color = Color.red;
    //    yield return new WaitForSeconds(1);
    //    SpriteRenderer.color = original;

    //}
}
