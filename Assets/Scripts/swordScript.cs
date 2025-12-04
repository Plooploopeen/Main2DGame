using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class swordScript : MonoBehaviour

    
{
    public Vector2 currentVelocity;
    private Vector2 stuckPosision;
    public PlayerSwordThrowingScript playerSwordThrowingScript;
    public bool isStuck = false;
    private int bounceCount = 0;
    private SpriteRenderer spriteRenderer;
    private HitBox hitBoxScript;
    [SerializeField] float swordStuckSnapDistance;
    [SerializeField] float stabAmount;
    [SerializeField] float stuckSpeed;
    [SerializeField] int bounceCountLimit;
    [SerializeField] LayerMask excludedLayers;

    void Start()
    {

    }

    void Update()
    {
        if (bounceCount >= bounceCountLimit && !isStuck)
        {
            playerSwordThrowingScript.swordInstance.transform.position = Vector2.Lerp(playerSwordThrowingScript.swordInstance.transform.position, stuckPosision, stuckSpeed);

            if (Vector2.Distance(playerSwordThrowingScript.swordInstance.transform.position, stuckPosision) < swordStuckSnapDistance)
            {
                playerSwordThrowingScript.swordRb.bodyType = RigidbodyType2D.Kinematic;
                playerSwordThrowingScript.swordRb.linearVelocity = Vector2.zero;
                playerSwordThrowingScript.velocity = Vector2.zero;
                isStuck = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bounce(collision);
    }

    public void bounce(Collision2D collision)
    {


        bounceCount++;

        hitBoxScript.hitEnemies.Clear();

        if (bounceCount >= bounceCountLimit)
        {
            playerSwordThrowingScript.swordRb.excludeLayers = excludedLayers;
            playerSwordThrowingScript.swordRb.linearVelocity = playerSwordThrowingScript.velocity;
            stuckPosision = playerSwordThrowingScript.swordInstance.transform.position + (Vector3)(playerSwordThrowingScript.velocity.normalized * stabAmount);

            // make sword drawn behind ground
            spriteRenderer.sortingLayerName = "SwordStuck";

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

    }
}
