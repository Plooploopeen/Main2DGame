using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogueLineEvent
{
    [HideInInspector] public string dialogueText;

    [SerializeField] UnityEvent onDialogueLine;
    public UnityEvent OnDialogueLine => onDialogueLine;
}
