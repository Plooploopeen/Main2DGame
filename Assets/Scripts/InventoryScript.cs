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

public class InventoryScript : MonoBehaviour    
{

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public static InventoryScript instance;
    public Transform inventoryParent;
    public Transform hotbarParent;

    public List<Item> items = new List<Item>();

    [SerializeField] InputActionAsset inputActions;

    private List<SpellSlotScript> createdSlotScripts = new List<SpellSlotScript>();
    private Item selectedItem;
    private Item replacedItem;

    SpellSlotScript[] inventorySlots;
    public SpellSlotScript[] hotbarSlots;

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

    private int selectorIndex = 0;
    private int currentSlot = 0;
    private int selectedInventorySlotIndex;
    private int slotChangeAmount;

    private bool isInInventory = true;
    private bool isInHotbar;
    private bool justMovedToHotbar;
    private bool isUnequipping;

    private void Awake()
    {
        menuAction = InputSystem.actions.FindAction("Menu");
        DPadAction = InputSystem.actions.FindAction("DPad");
        acceptAction = InputSystem.actions.FindAction("Focus");
        backAction = InputSystem.actions.FindAction("Jump");

        inventorySlots = inventoryParent.GetComponentsInChildren<SpellSlotScript>();
        hotbarSlots = hotbarParent.GetComponentsInChildren<SpellSlotScript>();

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
        if (!inventory.activeSelf)
        {
            selectorIndex = 0;
            slotSelector.transform.position = spellSlots[0].transform.position;
            isInInventory = true;
            isInHotbar = false;
        }
        
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

        if (backAction.WasPressedThisFrame() && isInInventory && inventory.activeSelf)
        {
            toggleMenu();
        }

        if (isInHotbar && backAction.WasPressedThisFrame())
        {
            moveToInventory();
        }

        if (isInHotbar && !isUnequipping && acceptAction.WasPressedThisFrame())
        {
            equipSpell();
        }

        if (isInHotbar && isUnequipping && acceptAction.WasPressedThisFrame())
        {
            unequipSpell();
        }

        justMovedToHotbar = false;
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
        int limit = hotbarSlots.Length - 1;

        if (DPadDirection.x > 0)
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
        else if (DPadDirection.x < 0)
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

    void toggleMenu()
    { 
            inventory.SetActive(!inventory.activeSelf);

            if (inventory.activeSelf) { Time.timeScale = 0f; }
            else if (!inventory.activeSelf) { Time.timeScale = 1f; }
    }

    void moveToHotbar()
    {            
        selectedInventorySlotIndex = selectorIndex;
        slotSelector.transform.position = hotbarSlots[0].transform.position;
        isInHotbar = true;
        isInInventory = false;
        justMovedToHotbar = true;

        if (inventorySlots[selectedInventorySlotIndex].getItem() != null)
        {

            selectedItem = inventorySlots[selectorIndex].getItem();

        }
        else
        {
            isUnequipping = true;
        }

        selectorIndex = 0;
    }

    void moveToInventory()
    {
        slotSelector.transform.position = spellSlots[0].transform.position;
        selectorIndex = 0;
        isInHotbar = false;
        isInInventory = true;
        isUnequipping = false;
    }

    void equipSpell()
    {
        if (!justMovedToHotbar)
        {

            replacedItem = hotbarSlots[selectorIndex].getItem();
                
            // if slot is not null, replace it and put previous item back in intentory
            if (hotbarSlots[selectorIndex].getItem() != null)
            {
                inventorySlots[selectedInventorySlotIndex].addItem(replacedItem);
                items[selectedInventorySlotIndex] = replacedItem;
            }
            else
            {
                inventorySlots[selectedInventorySlotIndex].clearSlot();

                items.RemoveAt(selectedInventorySlotIndex);
                // cycle through inventory slots after moved one in inventory and move them back one
                for (int i = selectedInventorySlotIndex + 1; i < slotCount; i++)
                {
                    Item movedItem = inventorySlots[i].getItem();
                    if (movedItem != null)
                    {
                        inventorySlots[i - 1].addItem(movedItem);
                    }
                    else
                    {
                        inventorySlots[i - 1].clearSlot();
                    }
                }

            }


            // add selected item to hotbar
            hotbarSlots[selectorIndex].addItem(selectedItem);

            // change bools
            isInHotbar = false;
            isInInventory = true;

            // move selector back to inventory
            slotSelector.transform.position = inventorySlots[selectedInventorySlotIndex].transform.position;
            selectorIndex = selectedInventorySlotIndex;
        }

 
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

    //-------------end of item manager---------------------//
    void unequipSpell()
    {
        if (!justMovedToHotbar)
        {
            if (hotbarSlots[selectorIndex].getItem() != null)
            {
                Item movedItem = hotbarSlots[selectorIndex].getItem();

                inventorySlots[selectedInventorySlotIndex].addItem(movedItem);
                items.Add(movedItem);

                hotbarSlots[selectorIndex].clearSlot();

                // change bools
                isInHotbar = false;
                isInInventory = true;

                // move selector back to inventory
                slotSelector.transform.position = inventorySlots[selectedInventorySlotIndex].transform.position;
                selectorIndex = selectedInventorySlotIndex;
            }
        }
    }
}
