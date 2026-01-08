using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.InputSystem;
using System.Linq;

public class DialogueUI : MonoBehaviour
{
    private InputAction rightAction;

    [SerializeField] GameObject dialogueBox;
    [SerializeField] private TMP_Text text;
    [SerializeField] private DialogueObject testDialogue;

    private ResponseHandler responseHandler;
    private TypewriterEffect typewriterEffect;

    private void Start()
    {
        responseHandler = GetComponent<ResponseHandler>();
        typewriterEffect = GetComponent<TypewriterEffect>();   
        closeDialogueBox();
        ShowDialogue(testDialogue);
    }

    private void Awake()
    {
        rightAction = InputSystem.actions.FindAction("Focus");
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            string dialogue = dialogueObject.Dialogue[i];
            yield return typewriterEffect.Run(dialogue, text);

            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) break;

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

    void closeDialogueBox()
    {
        dialogueBox.SetActive(false);
        text.text = string.Empty;
    }
}
