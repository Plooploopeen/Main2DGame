using UnityEngine;

[CreateAssetMenu(fileName = "New spell", menuName = "Inventory/Spell")]
public class Spell : ScriptableObject
{
    new public string name = "New spell";
    public Sprite icon;
    public int cost;
    public int decreaseCostAmount;
    public GameObject spellPrefab;
    public int minimumCost;

    public virtual void use()
    {
        Debug.Log("using " + name);
    }
}
