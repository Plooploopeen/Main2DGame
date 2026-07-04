using UnityEngine;
using UnityEngine.UI;

public class SpellSlotScript : MonoBehaviour
{
    public Image icon;

    Spell spell;

    public void addItem(Spell newItem)
    {
        spell = newItem;
        icon.enabled = true;
        icon.sprite = spell.icon;

        if (icon.sprite == null)
        {
            Debug.Log("icon sprite is null");
        }
    }

    public void clearSlot()
    {
        spell = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void useItem()
    {
        if (spell != null)
        {
            spell.use();
        }
    }

    public Spell getItem()
    {
        return spell;
    }
}

