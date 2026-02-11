using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [SerializeField][TextArea] public string sentence;

    [SerializeField] public string speakerName;
}
