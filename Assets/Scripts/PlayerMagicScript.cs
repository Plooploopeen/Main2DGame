using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PlayerMagicScript : MonoBehaviour
{
    PlayerScript playerScript;
    [SerializeField] HotbarScript hotbarScript;
    Item selectedSpell;
    [SerializeField] Text textReduction;

    [SerializeField] GameObject inventory;

    public Transform hotbarParent;
    SpellSlotScript[] hotbarSlots;

    public int currentMP;
    public int maxMP;
    private int cost;
    private float interval = 1;
    private bool isFocusing;
    private float focusTime = 0;

    bool isGrounded => playerScript.IsGrounded;

    private InputAction focusAction;

    private void Awake()
    {
        playerScript = GetComponent<PlayerScript>();
        focusAction = InputSystem.actions.FindAction("Focus");

        hotbarSlots = hotbarParent.GetComponentsInChildren<SpellSlotScript>();
    }

    void Start()
    {
        textReduction.text = "";
    }

    void Update()
    {
        if (isGrounded && focusAction.WasPressedThisFrame() && playerScript.moveDirection.x == 0 && !inventory.activeSelf)
        {
            isFocusing = true;
            selectedSpell = hotbarSlots[hotbarScript.SelectorIndex].getItem();
            cost = selectedSpell.cost;
            textReduction.text = "-" + cost;
        }

        if (focusAction.WasReleasedThisFrame())
        {
            textReduction.text = "";
            isFocusing = false;
        }

        if (isFocusing)
        {
            focus();
        }
    }

    void focus()
    {
            focusTime += Time.deltaTime;
            
            if (focusTime >= interval)
            {
                focusTime -= interval;
                cost -= selectedSpell.decreaseCostAmount;
                textReduction.text = "-" + cost;
                
            }

    }
}