using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable target = collision.gameObject.GetComponent<IDamageable>();

        if (target != null)
        {
            target.takeDamage(damage);
        }
    }
}
