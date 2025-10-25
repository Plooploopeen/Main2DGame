using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackScript : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions;
    [SerializeField] GameObject weaponHitbox;

    private InputAction attackAction;
    private bool isAttacking = false;
    private bool canAttack = true;

    private void Awake()
    {
        attackAction = InputSystem.actions.FindAction("Attack");
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (attackAction.WasPressedThisFrame() && canAttack)
        {
            attack();
        }
    }

    void attack()
    {
        isAttacking = true;
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
    }
}
