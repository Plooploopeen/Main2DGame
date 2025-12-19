using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMagicScript : MonoBehaviour
{
    PlayerScript playerScript;

    [SerializeField] GameObject inventory;

    public int currentMP = 10;
    public int maxMP = 10;
    Item selectedSpell;
    bool isGrounded => playerScript.IsGrounded;

    private InputAction focusAction;

    private void Awake()
    {
        playerScript = GetComponent<PlayerScript>();
        focusAction = InputSystem.actions.FindAction("Focus");
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (isGrounded && focusAction.IsPressed() && playerScript.moveDirection.x == 0 && !inventory.activeSelf)
        {

        }
    }

    public void getSpell(Item spell)
    {
        Debug.Log(spell.name);
        selectedSpell = spell;
    }

}
