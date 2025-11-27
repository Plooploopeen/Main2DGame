using UnityEngine;

public interface IDamageable
{
    void takeDamage(float damage, Transform attackerTransform);
    void die();
}