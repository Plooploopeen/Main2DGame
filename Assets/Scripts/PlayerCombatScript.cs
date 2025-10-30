using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackScript : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions;
    [SerializeField] GameObject weaponHitbox;

    private Animator animator;

    private InputAction attackAction;
    public bool isAttacking = false;
    private bool canAttack = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        attackAction = InputSystem.actions.FindAction("Attack");
    }

    void Start()
    {
        weaponHitbox.SetActive(false);
    }

    void Update()
    {
        checkIfAttacking();
    }

    void attack()
    {
        isAttacking = true;
        animator.Play("Attacks.TestAttack");
    }

    public void enableHitbox()
    {
        weaponHitbox.SetActive(true);
    }

    public void disableHitbox()
    {
        weaponHitbox.SetActive(false);
    }

    public void attackCompleted()
    {
        isAttacking = false;
        animator.Play("Idle");
    }

    void checkIfAttacking()
    {
        if (attackAction.WasPressedThisFrame() && canAttack)
        {
            attack();
        }
    }
}
