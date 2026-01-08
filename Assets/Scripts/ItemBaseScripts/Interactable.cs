using UnityEngine;

public class PickUpable : MonoBehaviour
{
    public float radius = 3f;

    [SerializeField] Transform playerTransform;

    private bool hasPickedUp;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public virtual void interactWithItem()
    {

    }

    private void Update()
    {
        float distance = Vector2.Distance(playerTransform.position, transform.position);
        if (distance < radius &&  !hasPickedUp)
        {
            hasPickedUp = true;
            interactWithItem();
        }
    }
}
