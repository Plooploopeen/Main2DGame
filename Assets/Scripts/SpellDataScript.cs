using UnityEngine;

[CreateAssetMenu(fileName = "NewSpell", menuName = "Spells/Spell Data")]
public class SpellDataScript : ScriptableObject
{
    public string spellName;
    public Sprite spellIcon;
    public float spellDamage;
    public float spellMpCost;
    public GameObject spellPrefab;
}
