using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Rendering;

public class InventoryScript : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions;

    [SerializeField] GameObject inventory;
    [SerializeField] GameObject[] spellSlots;
    [SerializeField] GameObject[] hotbarSlots;
    [SerializeField] SpriteRenderer slotSelector;

    private InputAction menuAction;
    private InputAction DPadAction;
    private InputAction acceptAction;
    private InputAction backAction;

    private int selectorIndex = 0;
    private int currentSlot = 0;
    private int slotChangeAmount;

    private bool isInInventory;
    private bool isInHotbar;

    private void Awake()
    {
        menuAction = InputSystem.actions.FindAction("Menu");
        DPadAction = InputSystem.actions.FindAction("DPad");
        acceptAction = InputSystem.actions.FindAction("Focus");
        backAction = InputSystem.actions.FindAction("Jump");


    }

    void Start()
    {
        inventory.SetActive(false);
        StartCoroutine(ResetSelectorNextFrame());

    }

    void Update()
    {
        if (inventory.activeSelf && DPadAction.WasPressedThisFrame() && isInInventory)
        {
            moveInventorySlot();
        }

        if (menuAction.WasPressedThisFrame())
        {
            toggleMenu();
        }
                
        checkInInventory();

        if (DPadAction.WasPressedThisFrame() && isInHotbar)
        {
            moveHotbarSlot();
        }

        checkInHotbar();

        if (isInInventory && acceptAction.WasPressedThisFrame())
        {
            moveToHotbar();
        }

        if (isInHotbar && backAction.WasPressedThisFrame())
        {
            moveToInventory();
        }
    }

    private void OnEnable()
    {
        StartCoroutine(ResetSelectorNextFrame());
    }

    private void OnDisable()
    {
        
    }

    IEnumerator ResetSelectorNextFrame()
    {
        yield return null;
        slotSelector.transform.position = spellSlots[0].transform.position;
        selectorIndex = 0;
    }

    void changeSpellSlot(int currentSlot, int slotChangeAmount)
    {
        slotSelector.transform.position = spellSlots[currentSlot + slotChangeAmount].transform.position;
        selectorIndex = currentSlot + slotChangeAmount;
    }

    void changeHotbarSlot (int currentSlot, int slotChangeAmount)
    {
        slotSelector.transform.position = hotbarSlots[currentSlot + slotChangeAmount].transform.position;
        selectorIndex = currentSlot + slotChangeAmount;
    }


    void moveInventorySlot()
    {
        Vector2 DPadDirection = DPadAction.ReadValue<Vector2>();

            if (DPadDirection.x > 0)
            {
                currentSlot = selectorIndex;
                slotChangeAmount = 1;
                changeSpellSlot(currentSlot, slotChangeAmount);
            }
            else if (DPadDirection.x < 0)
            {
                currentSlot = selectorIndex;
                slotChangeAmount = -1;
                changeSpellSlot(currentSlot, slotChangeAmount);
            }
            else if (DPadDirection.y > 0)
            {
                currentSlot = selectorIndex;
                slotChangeAmount = -3;
                changeSpellSlot(currentSlot, slotChangeAmount);
            }
            else if (DPadDirection.y < 0)
            {
                currentSlot = selectorIndex;
                slotChangeAmount = 3;
                changeSpellSlot(currentSlot, slotChangeAmount);
            }
    }

    void moveHotbarSlot()
    {
        Vector2 DPadDirection = DPadAction.ReadValue<Vector2>();

        if (DPadDirection.x > 0)
        {
            currentSlot = selectorIndex;
            slotChangeAmount = 1;
            changeHotbarSlot(currentSlot, slotChangeAmount);
        }
        else if (DPadDirection.x < 0)
        {
            currentSlot = selectorIndex;
            slotChangeAmount = -1;
            changeHotbarSlot(currentSlot, slotChangeAmount);
        }
    }

    void toggleMenu()
    { 
            inventory.SetActive(!inventory.activeSelf);

            if (inventory.activeSelf) { Time.timeScale = 0f; }
            else if (!inventory.activeSelf) { Time.timeScale = 1f; }
    }

    void checkInInventory()
    {
        foreach (GameObject slot in spellSlots)
        {
            if (slotSelector.transform.position == slot.transform.position && inventory.activeSelf)
            {
                isInInventory = true;
                break;
            }
            else
            {
                isInInventory = false;
            }
        }
    }

    void checkInHotbar()
    {
        foreach (GameObject slot in hotbarSlots)
        {
            if (slotSelector.transform.position == slot.transform.position)
            {
                isInHotbar = true;
                break;
            }
            else
            {
                isInHotbar = false;
            }
        }
    }

    void moveToHotbar()
    {
        slotSelector.transform.position = hotbarSlots[0].transform.position;
        isInHotbar = true;
        selectorIndex = 0;
    }

    void moveToInventory()
    {
        slotSelector.transform.position = spellSlots[0].transform.position;
        selectorIndex = 0;
    }
}
