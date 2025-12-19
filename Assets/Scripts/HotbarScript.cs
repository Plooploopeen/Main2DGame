using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class HotbarScript : MonoBehaviour
{
    public PlayerMagicScript playerMagicScript;
    public InventoryScript inventoryScript;

    public static InventoryScript instance;
    public Transform hotbarParent;

    [SerializeField] InputActionAsset inputActions;

    SpellSlotScript[] hotbarSlots;

    [SerializeField] GameObject inventory;
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject spellSlotPrefab;
    [SerializeField] GameObject[] spellSlots;
    [SerializeField] SpriteRenderer slotSelector;
    [SerializeField] int slotCount;

    private InputAction menuAction;
    private InputAction DPadAction;
    private InputAction acceptAction;
    private InputAction backAction;
    private InputAction LB;
    private InputAction RB;
    private InputAction focusAction;

    private int selectorIndex = 0;
    private int currentSlot = 0;
    private int slotChangeAmount;

    private void Awake()
    {
        hotbarSlots = hotbarParent.GetComponentsInChildren<SpellSlotScript>();
        LB = InputSystem.actions.FindAction("LB");
        RB = InputSystem.actions.FindAction("RB");
        focusAction = InputSystem.actions.FindAction("Focus");


    }

    void Update()
    {
        if (inventory.activeSelf)
        {
            slotSelector.enabled = false;
        }
        else
        {
            slotSelector.enabled = true;
        }

        if (!inventory.activeSelf)
        {
            moveHotbarSlot();
        }

        if (focusAction.WasPressedThisFrame() && slotSelector.enabled)
        {
            Item spell = inventoryScript.hotbarSlots[selectorIndex].getItem();
            if (spell != null)
            {
                playerMagicScript.getSpell(spell);
            }
        }
    }

    void changeHotbarSlot(int currentSlot, int slotChangeAmount)
    {
        slotSelector.transform.position = hotbarSlots[currentSlot + slotChangeAmount].transform.position;
        selectorIndex = currentSlot + slotChangeAmount;
    }

    void moveHotbarSlot()
    {
        int limit = hotbarSlots.Length - 1;

        if (RB.WasPressedThisFrame())
        {
            if (selectorIndex == limit)
            {
                selectorIndex = 0;
                slotSelector.transform.position = hotbarSlots[0].transform.position;
                return;
            }
            currentSlot = selectorIndex;
            slotChangeAmount = 1;
            changeHotbarSlot(currentSlot, slotChangeAmount);
        }
        else if (LB.WasPressedThisFrame())
        {
            if (selectorIndex == 0)
            {
                selectorIndex = limit;
                slotSelector.transform.position = hotbarSlots[limit].transform.position;
                return;
            }
            currentSlot = selectorIndex;
            slotChangeAmount = -1;
            changeHotbarSlot(currentSlot, slotChangeAmount);
        }
    }


}