using Unity.VisualScripting;
using UnityEngine;

public class ShieldEnemyHealth : EnemyHealth
{
    public override void takeDamage(float damage, Transform attackerTransform)
    {
        float attackerDirection = Mathf.Sign(attackerTransform.position.x - transform.root.position.x);
        float enemyFacing = Mathf.Sign(transform.root.localScale.x);

        if (attackerDirection == enemyFacing)
        {
            return;
        }

        base.takeDamage(damage, attackerTransform);
    }
}
