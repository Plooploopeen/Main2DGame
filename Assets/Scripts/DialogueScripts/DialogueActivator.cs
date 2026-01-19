using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable
{

    [SerializeField] DialogueObject dialogueObject;

    public void UpdateDialogueObject(DialogueObject dialogueObject)
    {
        this.dialogueObject = dialogueObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.TryGetComponent(out PlayerScript playerScript))
        {
            playerScript.Interactable = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.TryGetComponent(out PlayerScript playerScript))
        {
            if (playerScript.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this)
            {
                playerScript.Interactable = null;
            }
        }

    }

    public void Interact(PlayerScript playerScript)
    {
        foreach(DialogueResponseEvents responseEvents in GetComponents<DialogueResponseEvents>())
        {
            if (responseEvents.DialogueObject == dialogueObject)
            {
                playerScript.DialogueUI.AddResponseEvents(responseEvents.Events);
                playerScript.DialogueUI.AddDialogueLineEvents(responseEvents.DialogueLineEvents);
                break;
            }
        }

        playerScript.DialogueUI.ShowDialogue(dialogueObject);
    }
}
