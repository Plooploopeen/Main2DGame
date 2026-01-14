using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueUI : MonoBehaviour
{
    private InputAction rightAction;

    [SerializeField] GameObject dialogueBox;
    [SerializeField] private TMP_Text text;

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
        rightAction = InputSystem.actions.FindAction("Focus");
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        isOpen = true;
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            string dialogue = dialogueObject.Dialogue[i];

            yield return RunTypingEffect(dialogue);

            text.text = dialogue;

            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) break;

            yield return null;
            yield return new WaitUntil(() => rightAction.WasPressedThisFrame());
        }

        if (dialogueObject.HasResponses)
        {
            responseHandler.ShowResponses(dialogueObject.Responses);
        }
        else
        {
            closeDialogueBox();
        }
    }

    private IEnumerator RunTypingEffect(string dialogue)
    {
        typewriterEffect.Run(dialogue, text);

        while (typewriterEffect.isRunning)
        {
            yield return null;

            if (rightAction.WasPressedThisFrame())
            {
                typewriterEffect.Stop();
            }
        }
    }

    void closeDialogueBox()
    {
        isOpen = false;
        dialogueBox.SetActive(false);
        text.text = string.Empty;
    }
}
