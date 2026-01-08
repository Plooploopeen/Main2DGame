using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;

    InventoryScript inventory;

    SpellSlotScript[] slots;

    void Start()
    {
        inventory = InventoryScript.instance;
        inventory.onItemChangedCallback += updateUI;
        slots = itemsParent.GetComponentsInChildren<SpellSlotScript>();
    }

    void Update()
    {

    }

    void updateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].addItem(inventory.items[i]);
            }
        }
    }
}

