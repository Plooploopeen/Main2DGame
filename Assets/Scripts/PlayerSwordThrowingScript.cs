using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSwordThrowingScript : MonoBehaviour
{
    [SerializeField] GameObject swordPrefafab;

    private InputAction aimAction;
    private InputAction throwAction;

    private LineRenderer lineRenderer;

    private bool isAiming;
    private Vector2 startPoint;
    private Vector2 endPoint;
    private Vector2 aimDirection;
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

        checkThrow();
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

    void checkThrow()
    {
        if (aimAction.IsPressed() && throwAction.IsPressed())
        {
            Debug.Log("Throw");
        }
    }
}
