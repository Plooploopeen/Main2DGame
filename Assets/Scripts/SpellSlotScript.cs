using UnityEngine;

public class SpellSlotScript : MonoBehaviour
{
    private SpellDataScript spellData;
    public void setUp(SpellDataScript spell)
    {
        spellData = spell;

        spellData.spellIcon = spell.spellIcon;

    }

    public void setEmpty()
    {

        spellData = null;

        if (spellData != null)
        {
            // spellSprite is disabled
            spellData.spellIcon = null;
        }
    }
}
