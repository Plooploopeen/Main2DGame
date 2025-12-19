using UnityEngine;

public class PlayerMagicScript : MonoBehaviour
{
    public int currentMP = 10;
    public int maxMP = 10;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void castSpell(Item spell)
    {
        Debug.Log("Cast spell" + spell.name);
    }
}
