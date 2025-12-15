using UnityEngine;
using UnityEngine.UI;

public class SpellSlotScript : MonoBehaviour
{
    public Image icon;

    Item item;

    public void addItem(Item newItem)
    {
        item = newItem;
        icon.enabled = true;
        icon.sprite = item.icon;

        if (icon.sprite == null)
        {
            Debug.Log("icon sprite is null");
        }
    }

    public void clearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void useItem()
    {
        if (item != null)
        {
            item.use();
        }
    }

    public Item getItem()
    {
        return item;
    }
}

