using NUnit.Framework.Internal.Commands;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class swordScript : MonoBehaviour
{
    private PlayerMagicScript playerMagicScript;

    public Vector2 currentVelocity;
    private Vector2 stuckPosition;
    public PlayerSwordThrowingScript playerSwordThrowingScript;
    public bool isStuck = false;
    private int bounceCount = 0;
    private SpriteRenderer spriteRenderer;
    private HitBox hitBoxScript;
    [SerializeField] float maxDistance;
    [SerializeField] float swordStuckSnapDistance;
    [SerializeField] float stabAmount;
    [SerializeField] float stuckSpeed;
    [SerializeField] int bounceCountLimit;
    [SerializeField] LayerMask excludedLayers;
    private Transform playerTransform;

    private Vector2 currentScale;
    private quaternion currentRotation;
    private bool isStuckInHurtbox = false;

    void Start()
    {

    }

    void Update()
    {
        if (bounceCount >= bounceCountLimit && !isStuck)
        {
            playerSwordThrowingScript.swordInstance.transform.position = Vector2.Lerp(playerSwordThrowingScript.swordInstance.transform.position, stuckPosition, stuckSpeed);

            if (Vector2.Distance(playerSwordThrowingScript.swordInstance.transform.position, stuckPosition) < swordStuckSnapDistance)
            {
                playerSwordThrowingScript.swordRb.bodyType = RigidbodyType2D.Kinematic;
                playerSwordThrowingScript.swordRb.linearVelocity = Vector2.zero;
                playerSwordThrowingScript.velocity = Vector2.zero;
                isStuck = true;
            }
        }

        bool gravityOn = false;

        // stop flying if sword is too far away from player
        if (Vector2.Distance(playerSwordThrowingScript.swordInstance.transform.position, playerTransform.position) > maxDistance)
        {
            playerSwordThrowingScript.swordRb.gravityScale = 0.5f;
            playerSwordThrowingScript.swordRb.freezeRotation = false;
            playerSwordThrowingScript.swordRb.excludeLayers &= ~playerSwordThrowingScript.playerLayer;
            gravityOn = true;
        }

        if (gravityOn)
        {
            bounceCount = 0;
        }

        if (isStuckInHurtbox)
        {
            playerSwordThrowingScript.swordInstance.transform.localScale = currentScale;
            playerSwordThrowingScript.swordInstance.transform.rotation = currentRotation;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            pickUpSword();
        }
        else
        {
            bounce(collision);
        }
    }

    public void bounce(Collision2D collision)
    {


        bounceCount++;

        hitBoxScript.hitEnemies.Clear();

        if (bounceCount >= bounceCountLimit)
        {
            playerSwordThrowingScript.swordRb.excludeLayers = excludedLayers;
            playerSwordThrowingScript.swordRb.linearVelocity = playerSwordThrowingScript.velocity;
            stuckPosition = playerSwordThrowingScript.swordInstance.transform.position + (Vector3)(playerSwordThrowingScript.velocity.normalized * stabAmount);

            // make sword drawn behind ground
            spriteRenderer.sortingLayerName = "SwordStuck";

            // set parent to stuck object. layer, remove this if statement and figure out scale problems so it can move with ground
            if (collision.gameObject.layer == LayerMask.NameToLayer("Hurtbox"))
            {
 
                playerSwordThrowingScript.swordInstance.transform.SetParent(collision.transform, true);
                currentScale = playerSwordThrowingScript.swordInstance.transform.localScale;
                currentRotation = playerSwordThrowingScript.swordInstance.transform.localRotation;
                isStuckInHurtbox = true;


                // gain mp if last hit collides with an enemy
                float gain = playerMagicScript.percentGain * playerMagicScript.maxMP;
                playerMagicScript.currentMP += gain;

            }

            return;
        }

        if (bounceCount == 1)
        {
            playerSwordThrowingScript.swordRb.excludeLayers &= ~playerSwordThrowingScript.playerLayer;
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
        spriteRenderer = GetComponent<SpriteRenderer>();
        hitBoxScript = GetComponentInChildren<HitBox>();
        playerTransform = playerSwordThrowingScript.GetComponentInParent<Transform>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        playerMagicScript = playerObject.GetComponent<PlayerMagicScript>();

    }

    void pickUpSword()
    {
        Destroy(playerSwordThrowingScript.swordInstance);
        playerSwordThrowingScript.canThrow = true;
        playerSwordThrowingScript.hasSword = true;

    }
}
