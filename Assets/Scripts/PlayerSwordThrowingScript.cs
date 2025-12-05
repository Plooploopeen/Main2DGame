using System.IO;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEditor.Experimental.GraphView;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSwordThrowingScript : MonoBehaviour
{
    public GameObject swordPrefab;
    [SerializeField] Transform swordTransform;
    private PlayerSwordThrowingScript playerSwordThrowingScript;


    private InputAction aimAction;
    private InputAction throwAction;
    public GameObject swordInstance;
    public Rigidbody2D swordRb;

    private LineRenderer lineRenderer;

    private bool isAiming;
    public bool canThrow = true;
    public bool isSwordFlying;
    public float speed;
    private Vector2 startPoint;
    private Vector2 endPoint;
    public Vector2 aimDirection;
    public Vector2 velocity;
    public LayerMask playerLayer;
    [SerializeField] float lineLength;


    private void Awake()
    {
        aimAction = InputSystem.actions.FindAction("Aim");
        throwAction = InputSystem.actions.FindAction("Throw");
        lineRenderer = GetComponent<LineRenderer>();
        playerSwordThrowingScript = GetComponent<PlayerSwordThrowingScript>();
        swordPrefab = Resources.Load<GameObject>("Prefabs/ThrownSword");
    }
    void Start()
    {

    }

    void Update()
    {

        aim();

        if (aimAction.IsPressed() && throwAction.WasPerformedThisFrame() && canThrow)
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


        if (isAiming)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, endPoint);
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
            swordScript swordScript = swordInstance.GetComponent<swordScript>();
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
            isSwordFlying = true;
            swordRb = swordInstance.GetComponent<Rigidbody2D>();

            // stop collison with player when thrown
            playerSwordThrowingScript.swordRb.excludeLayers = playerLayer;

            swordTransform = swordInstance.transform;
            velocity = aimDirection.normalized * speed;
            swordRb.linearVelocity = velocity;
    }
}
