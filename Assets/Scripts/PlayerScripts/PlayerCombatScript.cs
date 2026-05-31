using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackScript : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions;
    [SerializeField] GameObject weaponGameObject;
    [SerializeField] DialogueUI dialogueUI;
    private PlayerSwordThrowingScript playerSwordThrowingScript;

    private Animator animator;

    private InputAction attackAction;
    private InputAction LB;

    public bool isAttacking = false;
    public bool canAttack = true;
    private bool IsLastAttack;

    private void Awake()
    {
        attackAction = InputSystem.actions.FindAction("Attack");
        LB = InputSystem.actions.FindAction("LB");

        animator = GetComponent<Animator>();
        playerSwordThrowingScript = GetComponent<PlayerSwordThrowingScript>();
    }

    void Start()
    {
        weaponGameObject.SetActive(false);
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && animator.GetBool("attackQueued"))
        {
            animator.SetBool("attackQueued", false);
        }

        if (dialogueUI.isOpen)
        {
            return;
        }

        if (!playerSwordThrowingScript.hasSword)
        {
            canAttack = false;
        }
        else
        {
            canAttack = true;
        }

        if (isAttacking && attackAction.WasPressedThisFrame())
        {
            AttackQueued();
        }

        checkShouldAttack();
    }

    void attack()
    {
        isAttacking = true;
        animator.Play("Attacks.basicAttack1");
    }

    public void enableHitbox()
    {
        weaponGameObject.SetActive(true);

        HitBox hitBox = weaponGameObject.GetComponent<HitBox>();
        hitBox.SetHitStop(0.06f);
    }

    public void disableHitbox()
    {
        weaponGameObject.SetActive(false);
    }

    public void attackCompleted()
    {
        Debug.Log("attackQueued: " + animator.GetBool("attackQueued") + " | IsLastAttack: " + IsLastAttack);
        if (animator.GetBool("attackQueued") == false || IsLastAttack)
        {
            isAttacking = false;
            IsLastAttack = false;
        }
        animator.SetBool("attackQueued", false);
    }

    void checkShouldAttack()
    {
        if (attackAction.WasPressedThisFrame() && !LB.IsPressed() && canAttack && !animator.GetCurrentAnimatorStateInfo(0).IsName("Stop Sprinting Slide") && !isAttacking)
        {
            attack();
        }
    }

    public void AttackQueued()
    {
        animator.SetBool("attackQueued", true);
    }

    public void EndAttackQueued()
    {
        animator.SetBool("attackQueued", false);
    }

    public void EnableIsLastAttack()
    {
        IsLastAttack = true;
    }

    public void DisableIsLastAttack()
    {
        IsLastAttack = false;
    }
}
