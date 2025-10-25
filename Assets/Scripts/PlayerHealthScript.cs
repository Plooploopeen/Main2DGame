using UnityEngine;
using UnityEngine.Rendering;

public class PlayerHealthScript : MonoBehaviour, IDamageable
{
    public float health = 100;
    [SerializeField] float maxHealth = 100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            die();
        }
    }

    public void die()
    {
        Debug.Log("Player died");
    }
}
