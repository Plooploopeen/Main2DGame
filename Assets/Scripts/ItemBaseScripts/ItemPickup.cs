using UnityEngine;

public class ItemPickup : PickUpable
{
    public Spell item;

    public override void interactWithItem()
    {
        base.interactWithItem();

        pickUp();
    }

    void pickUp()
    {
        bool wasPickedUp = InventoryScript.instance.Add(item);

        if (wasPickedUp)
        {
            Destroy(gameObject);
        }
    }
}
