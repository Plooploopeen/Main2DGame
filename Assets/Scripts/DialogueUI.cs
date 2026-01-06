using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueUI : MonoBehaviour
{
    private InputAction rightAction;

    [SerializeField] GameObject dialogueBox;
    [SerializeField] private TMP_Text text;
    [SerializeField] private DialogueObject testDialogue;

    private TypewriterEffect typewriterEffect;

    private void Start()
    {
        closeDialogueBox();
        typewriterEffect = GetComponent<TypewriterEffect>();
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
        foreach (string dialogue in dialogueObject.Dialogue)
        {
            yield return typewriterEffect.Run(dialogue, text);
            yield return new WaitUntil(() => rightAction.WasPressedThisFrame());
        }

        closeDialogueBox();
    }

    void closeDialogueBox()
    {
        dialogueBox.SetActive(false);
        text.text = string.Empty;
    }
}
