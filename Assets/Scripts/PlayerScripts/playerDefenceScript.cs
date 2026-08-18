using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerDefenceScript : MonoBehaviour
{
    [SerializeField] DialogueUI dialogueUI;
    [SerializeField] InputActionAsset inputActions;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    PlayerMagicScript playerMagicScript;
    PlayerScript playerScript;

    private InputAction parryAction;

    private Color original;
    private bool canParry = true;
    public bool IsParrying;
    private float parryTime = 0;
    [SerializeField] float parryTimeLimit;
    [SerializeField] float parryCooldownAmount;
    private bool justParried;
    public bool justFinishedParry;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();

        parryAction = InputSystem.actions.FindAction("Parry");

        playerMagicScript = GetComponent<PlayerMagicScript>();
        playerScript = GetComponent<PlayerScript>();
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

        if (parryAction.WasPressedThisFrame() && canParry && !IsParrying && !GetComponent<PlayerAttackScript>().isAttacking)
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
        StartCoroutine(JustParriedFrame());
        //StartCoroutine(Flashpink());

    }

    void endParry()
    {
        StartCoroutine(ParryDirectionLock());
        StartCoroutine(ParryCooldown());
        IsParrying = false;
        parryTime = 0;

        animator.SetTrigger("parryEnd");
    }

    public void onParrySuccess(Transform attackerTransform)
    {
        IsParrying = false;
        parryTime = 0;

        StartCoroutine(ParryDirectionLock());
        playerScript.FaceTowards(attackerTransform);

        if (justParried)
        {
            StartCoroutine(ParrySlowDownTime(0.3f, 0.5f));

            float gain = playerMagicScript.percentGain * playerMagicScript.maxMP;
            playerMagicScript.currentMP += gain;

            StartCoroutine(Flashgold());
        }
        else
        {
            StartCoroutine(ParrySlowDownTime(0.4f, 0.25f));
        }

        animator.SetTrigger("parrySuccess");
    }

    IEnumerator ParryCooldown()
    {
        canParry = false;
        yield return new WaitForSeconds(parryCooldownAmount);
        canParry = true;
    }

    IEnumerator JustParriedFrame()
    {
        justParried = true;
        yield return new WaitForSeconds(0.1f);
        justParried = false;
    }

    IEnumerator ParrySlowDownTime(float slowScale, float duration)
    {
        Time.timeScale = slowScale;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
    }

    IEnumerator ParryDirectionLock()
    {
        justFinishedParry = true;
        yield return new WaitForSecondsRealtime(0.5f);
        justFinishedParry = false;
    }

    //IEnumerator Flashpink()
    //{
    //    Debug.Log("Flash pink");
    //    spriteRenderer.color = Color.pink;
    //    yield return new WaitForSeconds(parryTimeLimit);
    //    spriteRenderer.color = original;
    //}

    IEnumerator Flashgold()
    {
        spriteRenderer.color = Color.gold;
        yield return new WaitForSecondsRealtime(0.6f);
        spriteRenderer.color = original;
    }
}
