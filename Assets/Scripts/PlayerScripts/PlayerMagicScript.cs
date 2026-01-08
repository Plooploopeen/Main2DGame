using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerMagicScript : MonoBehaviour
{
    PlayerScript playerScript;
    [SerializeField] HotbarScript hotbarScript;
    Item selectedSpell;
    [SerializeField] Text textReduction;
    private SpriteRenderer spriteRenderer;

    [SerializeField] GameObject inventory;

    public Transform hotbarParent;
    SpellSlotScript[] hotbarSlots;

    private Color original;
    public float currentMP;
    public int maxMP;
    public float percentGain;
    private int cost;
    private float interval = 0.5f;
    private bool isFocusing;
    private float focusTime = 0;

    bool isGrounded => playerScript.IsGrounded;

    private InputAction LB;
    private InputAction RB;
    private InputAction rightAction;
    private InputAction bottomAction;
    private InputAction leftAction;
    private InputAction topAction;

    private void Awake()
    {
        playerScript = GetComponent<PlayerScript>();
        LB = InputSystem.actions.FindAction("LB");
        RB = InputSystem.actions.FindAction("RB");
        rightAction = InputSystem.actions.FindAction("Focus");
        bottomAction = InputSystem.actions.FindAction("Jump");
        leftAction = InputSystem.actions.FindAction("Attack");
        topAction = InputSystem.actions.FindAction("TopBotton");

        hotbarSlots = hotbarParent.GetComponentsInChildren<SpellSlotScript>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        textReduction.text = "";
        currentMP = maxMP;
        original = spriteRenderer.color;
    }

    void Update()
    {
        checkShouldFocus();

        if (isFocusing)
        {
            focus();
            hotbarScript.slotSelector.enabled = true;
            //spriteRenderer.color = Color.purple;
        }
        else
        {
            hotbarScript.slotSelector.enabled = false;
            focusTime = 0;
        }

        if (currentMP > maxMP)
        {
            currentMP = maxMP;
        }
    }

    void focus()
    {
            focusTime += Time.deltaTime;
            
            if (focusTime >= interval && selectedSpell != null)
            {
                focusTime -= interval;
                if (cost <= 0) { return;}
                cost -= selectedSpell.decreaseCostAmount;
                textReduction.text = "-" + cost;
                
            }
    }

    void castSpell()
    {
        if (selectedSpell != null)
        {
            if (cost <= currentMP)
            {
                currentMP -= cost;
            }
            else {return;}


            GameObject spellInstance = Instantiate(selectedSpell.itemPrefab, transform.position, Quaternion.identity);

            SpellBase spellScript = spellInstance.GetComponent<SpellBase>();

            if (spellScript != null)
            {
                spellScript.Initialize();
            }
        }
    }

    void checkShouldFocus()
    {
        if (isGrounded && playerScript.moveDirection.x == 0 && !inventory.activeSelf && LB.IsPressed() && (rightAction.WasPressedThisFrame() || bottomAction.WasPressedThisFrame() || leftAction.WasPressedThisFrame() || topAction.WasPressedThisFrame()))
        {
            StartCoroutine(StartFocusWithDelay());
        }
        else if (isFocusing && LB.IsPressed() && (rightAction.WasReleasedThisFrame() || bottomAction.WasReleasedThisFrame() || leftAction.WasReleasedThisFrame() || topAction.WasReleasedThisFrame()))
        {
            castSpell();
        }
        else if (!isGrounded || playerScript.moveDirection.x != 0 || inventory.activeSelf || (!rightAction.IsPressed() && !bottomAction.IsPressed() && !leftAction.IsPressed() && !topAction.IsPressed()))
        {
            isFocusing = false;
            textReduction.text = "";
        }
    }

    IEnumerator StartFocusWithDelay()
    {
        yield return null;
        selectedSpell = hotbarSlots[hotbarScript.SelectorIndex].getItem();
        if (selectedSpell != null)
        {
            isFocusing = true;
            cost = selectedSpell.cost;
            textReduction.text = "-" + cost;
        }
    }
}