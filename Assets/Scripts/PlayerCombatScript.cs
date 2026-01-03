using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackScript : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions;
    [SerializeField] GameObject weaponHitbox;
    private PlayerSwordThrowingScript playerSwordThrowingScript;

    private Animator animator;

    private InputAction attackAction;
    private InputAction LB;

    public bool isAttacking = false;
    public bool canAttack = true;

    private void Awake()
    {
        attackAction = InputSystem.actions.FindAction("Attack");
        LB = InputSystem.actions.FindAction("LB");

        animator = GetComponent<Animator>();
        playerSwordThrowingScript = GetComponent<PlayerSwordThrowingScript>();
    }

    void Start()
    {
        weaponHitbox.SetActive(false);
    }

    void Update()
    {
        if (!playerSwordThrowingScript.hasSword)
        {
            canAttack = false;
        }
        else
        {
            canAttack = true;
        }
        checkShouldAttack();
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

    void checkShouldAttack()
    {
        if (attackAction.WasPressedThisFrame() && !LB.IsPressed() && canAttack && !animator.GetCurrentAnimatorStateInfo(0).IsName("Stop Sprinting Slide"))
        {
            attack();
        }
    }
}
