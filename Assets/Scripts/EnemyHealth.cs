using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour, IDamageable
{
    private float health;
    [SerializeField] float maxHealth;

    private void Start()
    {
        health = maxHealth;
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
        Destroy(gameObject);
    }
}
