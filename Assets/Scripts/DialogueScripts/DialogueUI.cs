using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions;

    private InputAction acceptAction;

    [SerializeField] GameObject dialogueBox;
    [SerializeField] private TMP_Text text;
    [SerializeField] TMP_Text speakerName;

    private DialogueLineEvent[] currentDialogueLineEvents;

    public bool isOpen {  get; private set; }

    private ResponseHandler responseHandler;
    private TypewriterEffect typewriterEffect;

    private void Start()
    {
        responseHandler = GetComponent<ResponseHandler>();
        typewriterEffect = GetComponent<TypewriterEffect>();   

        closeDialogueBox();
    }

    private void Awake()
    {
        acceptAction = InputSystem.actions.FindAction("Accept");
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        isOpen = true;
        dialogueBox.SetActive(true);

        inputActions.FindActionMap("Gameplay").Disable();
        inputActions.FindActionMap("UI").Enable();

        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        responseHandler.AddResponseEvents(responseEvents);
    }

    public void AddDialogueLineEvents(DialogueLineEvent[] dialogueLineEvents)
    {
        currentDialogueLineEvents = dialogueLineEvents;
    }
    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            speakerName.text = dialogueObject.Dialogue[i].speakerName;

            string dialogue = dialogueObject.Dialogue[i].sentence;

            yield return RunTypingEffect(dialogue);

            text.text = dialogue;

            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) break;

            yield return null;
            yield return new WaitUntil(() => acceptAction.WasPressedThisFrame());

            if (currentDialogueLineEvents != null && i < currentDialogueLineEvents.Length)
            {
                currentDialogueLineEvents[i].OnDialogueLine?.Invoke();
            }
        }

        if (dialogueObject.HasResponses)
        {
            responseHandler.ShowResponses(dialogueObject.Responses);
        }
        else
        {
            currentDialogueLineEvents = null;
            closeDialogueBox();
        }
    }

    private IEnumerator RunTypingEffect(string dialogue)
    {
        typewriterEffect.Run(dialogue, text);

        while (typewriterEffect.isRunning)
        {
            yield return null;

            if (acceptAction.WasPressedThisFrame())
            {
                typewriterEffect.Stop();
            }
        }
    }

    public void closeDialogueBox()
    {
        isOpen = false;
        dialogueBox.SetActive(false);
        text.text = string.Empty;
        inputActions.FindActionMap("Gameplay").Enable();
        inputActions.FindActionMap("UI").Disable();
    }
}
