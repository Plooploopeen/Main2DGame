using UnityEngine;

public class AxeScript : MonoBehaviour
{
    Transform playerTransform;
    [SerializeField] float despawnDistance;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
         if (Vector2.Distance(transform.position, playerTransform.position) > despawnDistance)
         {
            Destroy(gameObject);
         }
    }
}
