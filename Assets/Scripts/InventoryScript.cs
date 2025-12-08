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
    [SerializeField] SpriteRenderer slotSelector;

    private InputAction menuAction;
    private InputAction DPadAction;

    private int cursorIndex = 0;
    private int currentSlot = 0;
    private int slotChangeAmount;

    private void Awake()
    {
        menuAction = InputSystem.actions.FindAction("Menu");
        DPadAction = InputSystem.actions.FindAction("DPad");
    }

    void Start()
    {
        inventory.SetActive(false);
        StartCoroutine(MoveSelectorNextFrame());

    }

    void Update()
    {
        if (menuAction.WasPressedThisFrame())
        {
            inventory.SetActive(!inventory.activeSelf);

            if (inventory.activeSelf) {Time.timeScale = 0f;}
            else if (!inventory.activeSelf) { Time.timeScale = 1f;}
        }

        Vector2 DPadDirection = DPadAction.ReadValue<Vector2>();

        if (inventory.activeSelf && DPadAction.WasPressedThisFrame())
        {
            if (DPadDirection.x > 0)
            {
                currentSlot = cursorIndex;
                slotChangeAmount = 1;
                changeSlot(currentSlot, slotChangeAmount);
            }
            else if (DPadDirection.x < 0)
            {
                currentSlot = cursorIndex;
                slotChangeAmount = -1;
                changeSlot(currentSlot, slotChangeAmount);
            }
            else if (DPadDirection.y > 0) 
            {
                currentSlot = cursorIndex;
                slotChangeAmount = -3;
                changeSlot(currentSlot, slotChangeAmount);
            }
            else if (DPadDirection.y < 0)
            {
                currentSlot = cursorIndex;
                slotChangeAmount = 3;
                changeSlot(currentSlot, slotChangeAmount);
            }
        }
    }

    private void OnEnable()
    {
        Debug.Log("Enabled");
        StartCoroutine(MoveSelectorNextFrame());

    }

    private void OnDisable()
    {
        Debug.Log("Disabled");
    }

    IEnumerator MoveSelectorNextFrame()
    {
        yield return null;
        slotSelector.transform.position = spellSlots[0].transform.position;
        cursorIndex = 0;
    }

    void changeSlot(int currentSlot, int slotChangeAmount)
    {
        slotSelector.transform.position = spellSlots[currentSlot + slotChangeAmount].transform.position;
        cursorIndex = currentSlot + slotChangeAmount;
    }
}
