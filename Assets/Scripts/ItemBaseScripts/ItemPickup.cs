using UnityEngine;

public class ItemPickup : PickUpable
{
    public Item item;

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
