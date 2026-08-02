using Unity.VisualScripting;
using UnityEngine;

public class pullSwordScript : SpellBase
{
    PlayerSwordThrowingScript playerSwordThrowingScript;

    [SerializeField] float pullSpeed;
    [SerializeField] float pickUpDistance;


    private void Start()
    {
        playerSwordThrowingScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSwordThrowingScript>();

        // if sword isn't in scene, destroy spell
        if (playerSwordThrowingScript.swordInstance == null)
        {
            Destroy(gameObject);
        }

        // disable all collision
        Collider2D[] allColliders = FindObjectsByType<Collider2D>(FindObjectsSortMode.None);
        foreach (Collider2D col in allColliders)
        {
            Physics2D.IgnoreCollision(playerSwordThrowingScript.swordInstance.GetComponent<Collider2D>(), col, true);
        }

        // make sword show up on top of other objects
        playerSwordThrowingScript.swordInstance.GetComponent<SpriteRenderer>().sortingOrder = 10;

        // tried to get gravity to affect it so the sword rotates but it didn't work as well as I wanted it to\
        playerSwordThrowingScript.swordInstance.GetComponent<swordScript>().GravityOn = true;
        playerSwordThrowingScript.swordRb.bodyType = RigidbodyType2D.Dynamic;
        playerSwordThrowingScript.swordRb.gravityScale = 1.0f;
    }

    private void Update()
    {
        {
            // move sword towards player
            Vector2 directon = (playerSwordThrowingScript.GetComponent<Transform>().position - playerSwordThrowingScript.swordInstance.transform.position);
            playerSwordThrowingScript.swordRb.linearVelocity = (directon / 2) * pullSpeed;

            // if sword is close enough to player, pick it up
            if (Vector2.Distance(playerSwordThrowingScript.transform.position, playerSwordThrowingScript.swordInstance.transform.position) < pickUpDistance)
            {
                playerSwordThrowingScript.swordInstance.GetComponent<swordScript>().pickUpSword();

                Destroy(gameObject);
            }
        }
    }



}
