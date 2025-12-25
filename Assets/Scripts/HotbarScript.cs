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
    public SpriteRenderer slotSelector;
    [SerializeField] int slotCount;

    private InputAction menuAction;
    private InputAction DPadAction;
    private InputAction LB;
    private InputAction rightAction;
    private InputAction bottomAction;
    private InputAction leftAction;
    private InputAction topAction;

    private int selectorIndex = 0;
    public int SelectorIndex => selectorIndex;
    private int currentSlot = 0;
    private int slotChangeIndex;

    private void Awake()
    {
        hotbarSlots = hotbarParent.GetComponentsInChildren<SpellSlotScript>();
        LB = InputSystem.actions.FindAction("LB");
        rightAction = InputSystem.actions.FindAction("Focus");
        bottomAction = InputSystem.actions.FindAction("Jump");
        leftAction = InputSystem.actions.FindAction("Attack");
        topAction = InputSystem.actions.FindAction("TopBotton");
    }

    void Update()
    {
        if (!inventory.activeSelf)
        {
            moveHotbarSlot();
        }
    }

    void changeHotbarSlot(int slotChangeIndex)
    {
        slotSelector.transform.position = hotbarSlots[slotChangeIndex].transform.position;
        selectorIndex = slotChangeIndex;
    }

    void moveHotbarSlot()
    {
        if (LB.IsPressed())
        {
            if (topAction.WasPressedThisFrame())
            {
                selectorIndex = 0;
                slotSelector.transform.position = hotbarSlots[0].transform.position;
                currentSlot = selectorIndex;

                changeHotbarSlot(0);

            }
            else if (rightAction.WasPressedThisFrame())
            {
                selectorIndex = 1;
                slotSelector.transform.position = hotbarSlots[1].transform.position;
                currentSlot = selectorIndex;

                changeHotbarSlot(1);
            }
            else if (bottomAction.WasPressedThisFrame())
            {
                selectorIndex = 2;
                slotSelector.transform.position = hotbarSlots[2].transform.position;
                currentSlot = selectorIndex;

                changeHotbarSlot(2);
            }
            else if (leftAction.WasPressedThisFrame())
            {
                selectorIndex = 3;
                slotSelector.transform.position = hotbarSlots[3].transform.position;
                currentSlot = selectorIndex;

                changeHotbarSlot(3);
            }
        }
    }


}