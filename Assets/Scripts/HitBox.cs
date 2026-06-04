using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections;
using Unity.VisualScripting;

public class HitBox : MonoBehaviour
{
    playerDefenceScript playerDefenceScript;

    [SerializeField] BoxCollider2D hitBoxCollider;

    public float damage;

    public List<Collider2D> hitEnemies = new List<Collider2D>();

    public int hitCount => hitEnemies.Count;

    [SerializeField] LayerMask targetLayers;

    private Transform selfRoot;
    private float hitStopLength = 0;

    private void Awake()
    {
        selfRoot = transform.root;
        playerDefenceScript = GameObject.FindGameObjectWithTag("Player").GetComponent<playerDefenceScript>();
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
            StartCoroutine(HitStop());

            if (transform.parent != null)
            {
                target.takeDamage(damage, transform.parent.transform);
            }
            else
            {
                target.takeDamage(damage, transform);
            }
                hitEnemies.Add(collision);
        }
    }

    IEnumerator HitStop()
    {
        if (playerDefenceScript.IsParrying)
        {
            yield return null;
        }
        else
        {
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(hitStopLength);
            Time.timeScale = 1f;
        }

    }

    public void SetHitStop(float duration)
    {
        hitStopLength = duration;
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    //private void OnDrawGizmos()
    //{
    //    Vector2 center = hitBoxCollider.transform.position;
    //    Vector2 size = hitBoxCollider.size;
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(center, size);
    //}
}
