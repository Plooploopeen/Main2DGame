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
        checkShouldAttack();
    }

    void attack()
    {
        isAttacking = true;
        animator.Play("Attacks.TestAttack");
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
