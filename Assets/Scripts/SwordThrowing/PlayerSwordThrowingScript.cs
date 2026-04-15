using System.IO;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSwordThrowingScript : MonoBehaviour
{
    [SerializeField] DialogueUI dialogueUI;
    [SerializeField] Transform swordTransform;
    private PlayerSwordThrowingScript playerSwordThrowingScript;
    private GameObject swordPrefab;


    private InputAction aimAction;
    private InputAction LT;
    public GameObject swordInstance;
    public Rigidbody2D swordRb;
    private swordScript swordScript;

    private LineRenderer lineRenderer;

    private bool isAiming;
    public bool canThrow = true;
    public bool isSwordFlying;
    public bool hasSword = true;
    public float speed;
    private Vector2 startPoint;
    private Vector2 endPoint;
    public Vector2 aimDirection;
    public Vector2 velocity;
    public LayerMask playerLayer;
    [SerializeField] float lineLength;
    [SerializeField] float pickUpDistance;

    [System.NonSerialized]
    public float throwTime;

    [SerializeField] float throwTimeMinimum;


    private void Awake()
    {
        aimAction = InputSystem.actions.FindAction("Aim");
        LT = InputSystem.actions.FindAction("Throw");
        lineRenderer = GetComponent<LineRenderer>();
        playerSwordThrowingScript = GetComponent<PlayerSwordThrowingScript>();
        swordPrefab = Resources.Load<GameObject>("Prefabs/ThrownSword");
    }
    void Start()
    {

    }

    void Update()
    {
        throwTime += Time.deltaTime;


        if (dialogueUI.isOpen)
        {
            return;
        }

        aim();

        if (swordInstance != null)
        {
            if (LT.WasPressedThisFrame() && !canThrow && Vector2.Distance(swordInstance.transform.position, transform.position) <= pickUpDistance)
            {
                swordScript.pickUpSword();
            }
        }

        if (aimAction.IsPressed() && LT.WasPerformedThisFrame() && canThrow && throwTime >= throwTimeMinimum)
        {
            spawnSword();
        }
    }

    void aim()
    {
        if (aimAction.ReadValue<Vector2>().magnitude > 0.1)
        {
            isAiming = true;
        }
        else
        {
            isAiming = false;
        }

        aimDirection = aimAction.ReadValue<Vector2>().normalized;
        startPoint = transform.position;
        endPoint = transform.position + (Vector3)(aimDirection * lineLength);


        if (isAiming && Time.timeScale == 1f)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, endPoint);
            if (hasSword)
            {
                lineRenderer.startColor = Color.green;
                lineRenderer.endColor = Color.green;
            }
            else
            {
                lineRenderer.startColor = Color.red;
                lineRenderer.endColor = Color.red;
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    void spawnSword()
    {
            // spawn the sword
            swordInstance = Instantiate(swordPrefab, startPoint, Quaternion.identity);

            // initialize sword script
            swordScript = swordInstance.GetComponent<swordScript>();
            swordScript.Initialize(playerSwordThrowingScript);

            // compute direction
            Vector2 dir = (endPoint - startPoint).normalized;

            // convert direction to angle
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            // rotate the sword to face that direction
            swordInstance.transform.rotation = Quaternion.Euler(0, 0, angle - 90);

            //save direction for movemet/force later
            //var swordScript = sword.GetComponent<Sword>;
            //swordScript.travelDir = dir;

            canThrow = false;
            hasSword = false;
            isSwordFlying = true;
            swordRb = swordInstance.GetComponent<Rigidbody2D>();

            swordTransform = swordInstance.transform;
            velocity = aimDirection.normalized * speed;
            swordRb.linearVelocity = velocity;

            Physics2D.IgnoreCollision(swordInstance.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
    }
}
