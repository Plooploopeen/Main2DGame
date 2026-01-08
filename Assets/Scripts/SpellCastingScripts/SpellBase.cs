using UnityEngine;

public class SpellBase : MonoBehaviour
{
    protected PlayerScript playerScript;
    public void Initialize()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        playerScript = playerObject.GetComponent<PlayerScript>();
    }
}
