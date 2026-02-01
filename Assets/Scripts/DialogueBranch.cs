using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogueBranch
{
    public string BranchName;
    public DialogueLineEvent[] lineEvents;
}
