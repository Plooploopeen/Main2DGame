using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class swordScript : MonoBehaviour

    
{
    public Vector2 currentVelocity;
    public PlayerSwordThrowingScript playerSwordThrowingScript;
    public bool isStuck = false;
    private int bounceCount = 0;    
    [SerializeField] int bounceCountLimit;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bounce(collision);
    }

    void bounce(Collision2D collision)
    {


        bounceCount++;

        if (bounceCount >= bounceCountLimit)
        {
            playerSwordThrowingScript.swordRb.bodyType = RigidbodyType2D.Kinematic;
            playerSwordThrowingScript.swordRb.linearVelocity = Vector2.zero;

            isStuck = true;

            return;
        }

        var firstContact = collision.contacts[0].normal;
        currentVelocity = playerSwordThrowingScript.velocity;
        Vector2 newVelocity = Vector2.Reflect(currentVelocity, firstContact);
        playerSwordThrowingScript.velocity = newVelocity;
        playerSwordThrowingScript.swordRb.linearVelocity = playerSwordThrowingScript.velocity;

        float angle = Mathf.Atan2(newVelocity.y, newVelocity.x) * Mathf.Rad2Deg;
        playerSwordThrowingScript.swordInstance.transform.rotation = Quaternion.Euler(0, 0, angle - 90);

    }

    public void Initialize(PlayerSwordThrowingScript psts)
    {
        playerSwordThrowingScript = psts;
        currentVelocity = playerSwordThrowingScript.velocity;

    }
}
