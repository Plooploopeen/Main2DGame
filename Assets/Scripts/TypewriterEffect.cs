using System.Collections;
using UnityEditor;
using UnityEngine;
using TMPro;
using System;

public class TypewriterEffect : MonoBehaviour
{

    [SerializeField] float writingSpeed;
    public Coroutine Run(string textToType, TMP_Text text)
    {
        return StartCoroutine(TypeText(textToType, text));
    }

    IEnumerator TypeText(string textToType, TMP_Text text)
    {
        text.text = string.Empty;

        float t = 0;
        int charIndex = 0;

        while (charIndex < textToType.Length)
        {
            t += Time.deltaTime * writingSpeed;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            text.text =  textToType.Substring(0, charIndex);

            yield return null;
        }

        text.text = textToType;
    }
}
