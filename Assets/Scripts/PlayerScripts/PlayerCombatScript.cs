using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackScript : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions;
    [SerializeField] GameObject weaponGameObject;
    [SerializeField] DialogueUI dialogueUI;
    [SerializeField] HitBox swordHitBoxScript;
    private PlayerSwordThrowingScript playerSwordThrowingScript;
    private PlayerHealthScript playerHealthScript;
    private PlayerScript playerScript;
    private Animator animator;
    private Rigidbody2D rb;

    Vector2 lastSpriteLocalPosition;
    bool applyRootMotion;
    [SerializeField] Transform spriteChildTransform;

    private InputAction attackAction;
    private InputAction LB;

    public bool isAttacking = false;
    public bool canAttack = true;
    [SerializeField] float baseDamage;
    private bool IsLastAttack;
    public bool isLunging;

    private void Awake()
    {
        attackAction = InputSystem.actions.FindAction("Attack");
        LB = InputSystem.actions.FindAction("LB");

        animator = GetComponent<Animator>();
        playerSwordThrowingScript = GetComponent<PlayerSwordThrowingScript>();
        playerHealthScript = GetComponent<PlayerHealthScript>();
        playerScript = GetComponent<PlayerScript>();

        rb = GetComponent<Rigidbody2D>();


    }

    void Start()
    {
        weaponGameObject.SetActive(false);
        isAttacking = false;
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            isAttacking = false;
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
            animator.SetTrigger("attackPressed");
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
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attacks.basicAttack3"))
        {
            swordHitBoxScript.SetDamage(baseDamage * 3);
        }
        else
        {
            swordHitBoxScript.SetDamage(baseDamage);

        }

        weaponGameObject.SetActive(true); //this line

        swordHitBoxScript.SetHitStop(0.06f);
    }

    public void disableHitbox()
    {
        weaponGameObject.SetActive(false);
        swordHitBoxScript.SetDamage(baseDamage);
    }

    public void ClearAttackTrigger()
    {
        animator.ResetTrigger("attackPressed");
    }

    void checkShouldAttack()
    {
        if (attackAction.WasPressedThisFrame() && !LB.IsPressed() && canAttack && !animator.GetCurrentAnimatorStateInfo(0).IsName("Stop Sprinting Slide") && !isAttacking)
        {
            attack();
        }
    }

    public void LungeStep(float speed)
    {
        if (playerHealthScript.isKnockedBack || !playerScript.IsGrounded) { return; }
        isLunging = true;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        rb.AddForce(new Vector2(speed * Mathf.Sign(transform.localScale.x), 0), ForceMode2D.Impulse);
    }

    public void StopLunge()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        isLunging = false;
    }
}
