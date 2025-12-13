using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 3f;

    [SerializeField] Transform playerTransform;

    private bool hasInteracted;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public virtual void interact()
    {
        Debug.Log("Interacting with" + transform.name);
    }

    private void Update()
    {
        float distance = Vector2.Distance(playerTransform.position, transform.position);
        if (distance < radius &&  !hasInteracted)
        {
            hasInteracted = true;
            interact();
        }
    }
}
