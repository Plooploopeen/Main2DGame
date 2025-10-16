using UnityEngine;

public class DeathBarrierScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Dead");
            collision.transform.position = Vector2.zero;
        }
    }

}
