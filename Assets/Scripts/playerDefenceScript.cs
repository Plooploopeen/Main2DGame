using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerDefenceScript : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions;

    PlayerMagicScript playerMagicScript;

    private InputAction parryAction;

    private bool canParry = true;
    public bool isParrying;
    private float parryTime = 0;
    [SerializeField] float parryTimeLimit;
    [SerializeField] float parryCooldownAmount;

    private void Awake()
    {
        parryAction = InputSystem.actions.FindAction("Parry");

        playerMagicScript = GetComponent<PlayerMagicScript>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        if (parryAction.WasPressedThisFrame() && canParry)
        {
            startParry();
        }

        if (isParrying)
        {
            parryTime += Time.deltaTime;

            if (parryTime >= parryTimeLimit) { endParry(); }
        }

        Debug.Log(isParrying);
    }

    void startParry()
    {
        isParrying = true;
    }

    void endParry()
    {
        StartCoroutine(ParryCooldown());
        isParrying = false;
        parryTime = 0;
    }

    public void onParrySuccess()
    {
        isParrying = false;
        parryTime = 0;
        float gain = playerMagicScript.percentGain * playerMagicScript.maxMP;
        playerMagicScript.currentMP += gain;
    }

    IEnumerator ParryCooldown()
    {
        canParry = false;
        yield return new WaitForSeconds(parryCooldownAmount);
        canParry = true;
    }
}
