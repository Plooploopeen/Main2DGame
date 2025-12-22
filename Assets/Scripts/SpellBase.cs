using UnityEngine;

public class SpellBase : MonoBehaviour
{
    protected bool isFacingRight;
    public void Initialize(bool isRight)
    {   
        isFacingRight = isRight;
    }
}
