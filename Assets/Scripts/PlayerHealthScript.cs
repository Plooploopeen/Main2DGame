using UnityEngine;
using UnityEngine.Rendering;

public class PlayerHealthScript : MonoBehaviour, IDamageable
{
    public float health = 100;
    [SerializeField] float maxHealth = 100;

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

        if (health <= 0)
        {
            die();
        }
    }

    public void die()
    {

    }
}
