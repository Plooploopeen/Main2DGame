using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]

public class DialogueObject : ScriptableObject
{
    [SerializeField] private Dialogue[] dialogue;
    [SerializeField] Response[] responses;

    public Dialogue[] Dialogue => dialogue;

    public bool HasResponses => Responses != null && Responses.Length > 0;
    public Response[] Responses => responses;
}
