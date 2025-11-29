using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEditor.Experimental.GraphView;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSwordThrowingScript : MonoBehaviour
{
    [SerializeField] GameObject swordPrefab;
    [SerializeField] Transform swordTransform;


    private InputAction aimAction;
    private InputAction throwAction;
    private GameObject sword;
    [SerializeField] Rigidbody2D swordRb;

    private LineRenderer lineRenderer;

    private bool isAiming;
    private bool canThrow = true;
    private bool isSwordFlying;
    [SerializeField] float speed;
    private Vector2 startPoint;
    private Vector2 endPoint;
    private Vector2 aimDirection;
    private Vector2 velocity;
    [SerializeField] float lineLength;


    private void Awake()
    {
        aimAction = InputSystem.actions.FindAction("Aim");
        throwAction = InputSystem.actions.FindAction("Throw");
        lineRenderer = GetComponent<LineRenderer>();
    }
    void Start()
    {

    }

    void Update()
    {

        aim();

        if (aimAction.IsPressed() && throwAction.IsPressed() && canThrow)
        {
            spawnSword();
        }

        if (isSwordFlying)
        {
            moveSword();
        }

    }

    void aim()
    {
        if (aimAction.ReadValue<Vector2>().magnitude > 0.1)
        {
            Debug.Log("Aim");
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
            Debug.Log("Throw");
            // spawn the sword
            sword = Instantiate(swordPrefab, startPoint, Quaternion.identity);

            // compute direction
            Vector2 dir = (endPoint - startPoint).normalized;

            // convert direction to angle
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            // rotate the sword to face that direction
            sword.transform.rotation = Quaternion.Euler(0, 0, angle - 90);

            //save direction for movemet/force later
            //var swordScript = sword.GetComponent<Sword>;
            //swordScript.travelDir = dir;

            canThrow = false;
            isSwordFlying = true;
            swordRb = sword.GetComponent<Rigidbody2D>();
            swordTransform = sword.transform;
            velocity = aimDirection.normalized * speed;
    }

    void moveSword()
    {
        swordTransform.position += (Vector3)velocity * Time.deltaTime;
    }
}
