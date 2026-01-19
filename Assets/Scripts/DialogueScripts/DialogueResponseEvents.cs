using UnityEngine;
using System;

public class DialogueResponseEvents : MonoBehaviour
{
    [SerializeField] DialogueObject dialogueObject;
    [SerializeField] ResponseEvent[] events;
    [SerializeField] DialogueLineEvent[] dialogueLineEvents;

    public DialogueObject DialogueObject => dialogueObject;

    public ResponseEvent[] Events => events;
    public DialogueLineEvent[] DialogueLineEvents => dialogueLineEvents;

    public void OnValidate()
    {
        if (dialogueObject == null) return;
        if (dialogueObject.Responses == null) return;
        if (events != null && events.Length == dialogueObject.Responses.Length) return;

        if (events == null)
        {
            events = new ResponseEvent[dialogueObject.Responses.Length];
        }
        else
        {
            Array.Resize(ref events, dialogueObject.Responses.Length);
        }

            for (int i = 0; i < dialogueObject.Responses.Length; i++)
            {
                Response response = dialogueObject.Responses[i];

                if (events[i] != null)
                {
                    events[i].name = response.ResponseText;
                    continue;
                }

                events[i] = new ResponseEvent() { name = response.ResponseText };
            }


        if (dialogueObject.Dialogue != null)
        {
            if (dialogueLineEvents == null ||  dialogueLineEvents.Length != dialogueObject.Dialogue.Length)
            {
                Array.Resize(ref dialogueLineEvents, dialogueObject.Dialogue.Length);
            }

            for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
            {
                string dialogueLine = dialogueObject.Dialogue[i];

                if (dialogueLineEvents[i] == null)
                {
                    dialogueLineEvents[i] = new DialogueLineEvent();
                }
            }
        }
    }
}
