using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerDefenceScript : MonoBehaviour
{
    [SerializeField] DialogueUI dialogueUI;
    [SerializeField] InputActionAsset inputActions;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    PlayerMagicScript playerMagicScript;

    private InputAction parryAction;

    private Color original;
    private bool canParry = true;
    public bool IsParrying;
    private float parryTime = 0;
    [SerializeField] float parryTimeLimit;
    [SerializeField] float parryCooldownAmount;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        parryAction = InputSystem.actions.FindAction("Parry");

        playerMagicScript = GetComponent<PlayerMagicScript>();
    }
    void Start()
    {
        original = spriteRenderer.color;
    }

    void Update()
    {
        if (dialogueUI.isOpen)
        {
            return;
        }

        if (parryAction.WasPressedThisFrame() && canParry && !IsParrying)
        {
            startParry();
        }

        if (IsParrying)
        {
            parryTime += Time.deltaTime;

            if (parryTime >= parryTimeLimit) { endParry(); }
        }
    }

    void startParry()
    {
        IsParrying = true;
        //StartCoroutine(Flashpink());

    }

    void endParry()
    {
        StartCoroutine(ParryCooldown());
        IsParrying = false;
        parryTime = 0;

        animator.SetTrigger("parryEnd");
    }

    public void onParrySuccess()
    {
        IsParrying = false;
        parryTime = 0;
        float gain = playerMagicScript.percentGain * playerMagicScript.maxMP;
        playerMagicScript.currentMP += gain;
        //StartCoroutine(Flashgold());

        animator.SetTrigger("parrySuccess");
    }

    IEnumerator ParryCooldown()
    {
        canParry = false;
        yield return new WaitForSeconds(parryCooldownAmount);
        canParry = true;
    }

    //IEnumerator Flashpink()
    //{
    //    Debug.Log("Flash pink");
    //    spriteRenderer.color = Color.pink;
    //    yield return new WaitForSeconds(parryTimeLimit);
    //    spriteRenderer.color = original;
    //}

    //IEnumerator Flashgold()
    //{
    //    Debug.Log("Flash gold");
    //    spriteRenderer.color = Color.gold;
    //    yield return new WaitForSeconds(parryTimeLimit);
    //    spriteRenderer.color = original;
    //}
}
