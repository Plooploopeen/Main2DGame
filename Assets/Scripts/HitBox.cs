using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections;

public class HitBox : MonoBehaviour
{

    [SerializeField] BoxCollider2D hitBoxCollider;

    [SerializeField] float damage;

    public List<Collider2D> hitEnemies = new List<Collider2D>();

    public int hitCount => hitEnemies.Count;

    [SerializeField] LayerMask targetLayers;

    private Transform selfRoot;

    private void Awake()
    {
        selfRoot = transform.root;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TryDamage(collision);
    }

    private void OnEnable()
    {
        hitEnemies.Clear();

        CheckForHits();
    }

    void CheckForHits()
    {
        Vector2 center = hitBoxCollider.transform.position;
        Vector2 size = hitBoxCollider.size;

        Collider2D[] overlappingColliders = Physics2D.OverlapBoxAll(center, size, 0f, targetLayers);

        foreach (Collider2D collision in overlappingColliders)
        {
            TryDamage(collision);
        }
    }

    void TryDamage(Collider2D collision)
    {
        if (hitEnemies.Contains(collision)) return;

        IDamageable target = collision.gameObject.GetComponent<IDamageable>();

        if (collision.transform.root == selfRoot) return;

        if (target != null)
        {
            if (transform.parent != null)
            {
                target.takeDamage(damage, transform.parent.transform);
                StartCoroutine(HitStop());
            }
            else
            {
                target.takeDamage(damage, transform);
                StartCoroutine(HitStop());
            }
                hitEnemies.Add(collision);
        }
    }

    IEnumerator HitStop()
    {
        Debug.Log("Hitstop Activated");
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.06f);
        Time.timeScale = 1f;
    }

    //private void OnDrawGizmos()
    //{
    //    Vector2 center = hitBoxCollider.transform.position;
    //    Vector2 size = hitBoxCollider.size;
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(center, size);
    //}
}
