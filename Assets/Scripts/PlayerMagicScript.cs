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
    private float interval = 0.5f;
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
        currentMP = maxMP;
    }

    void Update()
    {
        if (isGrounded && focusAction.WasPressedThisFrame() && playerScript.moveDirection.x == 0 && !inventory.activeSelf)
        {
            isFocusing = true;
            selectedSpell = hotbarSlots[hotbarScript.SelectorIndex].getItem();
            if (selectedSpell != null)
            {
                cost = selectedSpell.cost;
                textReduction.text = "-" + cost;
            }
        }
        else if (isFocusing && focusAction.WasReleasedThisFrame())
        {
            castSpell();
        }
        else if (!isGrounded || !focusAction.IsPressed() || playerScript.moveDirection.x != 0 || inventory.activeSelf)
        {
            isFocusing = false;
            textReduction.text = "";
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
            else { return; }


            GameObject spellInstance = Instantiate(selectedSpell.itemPrefab, transform.position, Quaternion.identity);

            SpellBase spellScript = spellInstance.GetComponent<SpellBase>();

            if (spellScript != null)
            {
                spellScript.Initialize();
            }
        }
    }
}