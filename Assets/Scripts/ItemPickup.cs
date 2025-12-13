using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;

    public override void interact()
    {
        base.interact();

        pickUp();
    }

    void pickUp()
    {
        Debug.Log("Picking up " + item.name);
        Destroy(gameObject);
    }
}
