using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerDefenceScript : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions;
    private SpriteRenderer spriteRenderer;

    PlayerMagicScript playerMagicScript;

    private InputAction parryAction;

    private Color original;
    private bool canParry = true;
    public bool isParrying;
    private float parryTime = 0;
    [SerializeField] float parryTimeLimit;
    [SerializeField] float parryCooldownAmount;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        parryAction = InputSystem.actions.FindAction("Parry");

        playerMagicScript = GetComponent<PlayerMagicScript>();
    }
    void Start()
    {
        original = spriteRenderer.color;
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
    }

    void startParry()
    {
        isParrying = true;
        StartCoroutine(Flashpink());

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
        StartCoroutine(Flashgold());
    }

    IEnumerator ParryCooldown()
    {
        canParry = false;
        yield return new WaitForSeconds(parryCooldownAmount);
        canParry = true;
    }

    IEnumerator Flashpink()
    {
        Debug.Log("Flash pink");
        spriteRenderer.color = Color.pink;
        yield return new WaitForSeconds(parryTimeLimit);
        spriteRenderer.color = original;
    }

    IEnumerator Flashgold()
    {
        Debug.Log("Flash gold");
        spriteRenderer.color = Color.gold;
        yield return new WaitForSeconds(parryTimeLimit);
        spriteRenderer.color = original;
    }
}
