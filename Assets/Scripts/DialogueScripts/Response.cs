using UnityEngine;

[System.Serializable]
public class Response
{
    [SerializeField] string responseText;
    [SerializeField] DialogueObject dialogueObject;
    [SerializeField] string branchName;

    public string ResponseText => responseText;
    public DialogueObject DialogueObject => dialogueObject;
    public string BranchName => branchName;
}
