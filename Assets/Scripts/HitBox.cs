using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;

public class HitBox : MonoBehaviour
{
    private BoxCollider2D hitBoxCollider;

    [SerializeField] float damage;

    private List<Collider2D> hitEnemies = new List<Collider2D>();

    [SerializeField] LayerMask targetLayers;

    private Transform selfRoot;

    private void Awake()
    {
        hitBoxCollider = GetComponent<BoxCollider2D>();
        selfRoot = transform.root;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        tryDamage(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        tryDamage(collision);
    }

    private void OnEnable()
    {
        hitEnemies.Clear();

        checkForHits();
    }

    void checkForHits()
    {
        Vector2 center = hitBoxCollider.transform.position;
        Vector2 size = hitBoxCollider.size;

        Collider2D[] overlappingColliders = Physics2D.OverlapBoxAll(center, size, 0f, targetLayers);

        foreach (Collider2D collision in overlappingColliders)
        {
            tryDamage(collision);
        }
    }

    void tryDamage(Collider2D collision)
    {
        if (hitEnemies.Contains(collision)) return;


        IDamageable target = collision.gameObject.GetComponent<IDamageable>();

        if (collision.transform.root == selfRoot) return;

        if (target != null)
        {
            target.takeDamage(damage);
            hitEnemies.Add(collision);
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 center = hitBoxCollider.transform.position;
        Vector2 size = hitBoxCollider.size;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, size);
    }
}
