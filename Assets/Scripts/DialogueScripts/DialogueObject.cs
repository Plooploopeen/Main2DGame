using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]

public class DialogueObject : ScriptableObject
{
    [SerializeField][TextArea] private string[] dialogue;
    [SerializeField] Response[] responses;

    public string[] Dialogue => dialogue;

    public bool HasResponses => Responses != null && Responses.Length > 0;
    public Response[] Responses => responses;
}
