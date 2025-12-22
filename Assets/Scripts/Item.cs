using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New item";
    public Sprite icon;
    public int cost;
    public int decreaseCostAmount;
    public GameObject itemPrefab;

    public virtual void use()
    {
        Debug.Log("using " + name);
    }
}
