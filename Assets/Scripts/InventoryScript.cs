using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine.InputSystem.Utilities;

public class InventoryScript : MonoBehaviour    
{

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public static InventoryScript instance;    
    
    public List<Item> items = new List<Item>();

    [SerializeField] InputActionAsset inputActions;

    private List<SpellSlotScript> createdSlotScripts = new List<SpellSlotScript>();

    [SerializeField] GameObject inventory;
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject spellSlotPrefab;
    [SerializeField] GameObject[] spellSlots;
    [SerializeField] GameObject[] hotbarSlots;
    [SerializeField] SpriteRenderer slotSelector;
    [SerializeField] int slotCount;

    private InputAction menuAction;
    private InputAction DPadAction;
    private InputAction acceptAction;
    private InputAction backAction;

    private int selectorIndex = 0;
    private int currentSlot = 0;
    private int slotChangeAmount;

    private bool isInInventory = true;
    private bool isInHotbar;

    private void Awake()
    {
        menuAction = InputSystem.actions.FindAction("Menu");
        DPadAction = InputSystem.actions.FindAction("DPad");
        acceptAction = InputSystem.actions.FindAction("Focus");
        backAction = InputSystem.actions.FindAction("Jump");

        if (instance  != null)
        {
            Debug.Log("More than one instance of inventory found!");
            return;
        }

        instance = this;
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

        if (DPadAction.WasPressedThisFrame() && isInHotbar)
        {
            moveHotbarSlot();
        }

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

    void moveToHotbar()
    {
        slotSelector.transform.position = hotbarSlots[0].transform.position;
        selectorIndex = 0;
        isInHotbar = true;
        isInInventory = false;
    }

    void moveToInventory()
    {
        slotSelector.transform.position = spellSlots[0].transform.position;
        selectorIndex = 0;
        isInHotbar = false;
        isInInventory = true;
    }

    //---------item manager------------//

    public bool Add(Item item)
    {
        if (items.Count >= slotCount)
        {
            return false;
        }

        items.Add(item);

        if (onItemChangedCallback  != null)
        {
            onItemChangedCallback.Invoke();
        }   

        return true;
    }

    public void remove(Item item)
    {
        items.Remove(item);

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }
}
