using System.Collections;
using UnityEditor;
using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using NUnit.Framework.Api;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] float writingSpeed;

    private readonly Dictionary<HashSet<char>, float> punctuations = new Dictionary<HashSet<char>, float>()
    {
        {new HashSet<char>(){'.', '!', '?', }, 0.6f },
        {new HashSet<char>(){',', ';', ':', }, 0.3f },
    };
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
            int lastCharacterIndex = charIndex;

            t += Time.deltaTime * writingSpeed;

            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            for (int i = lastCharacterIndex; i < charIndex; i++)
            {
                bool isLast = i >= textToType.Length - 1;

                text.text = textToType.Substring(0, i + 1);

                if (IsPunctuation(textToType[i], out float waitTime) && !isLast && !IsPunctuation(textToType[i + 1], out _))
                {
                    yield return new WaitForSeconds(waitTime);
                }
            }

            yield return null;
        }

        text.text = textToType;
    }

    private bool IsPunctuation(char character, out float waitTime)
    {
        foreach(KeyValuePair<HashSet<char>, float> punctuationCategory in punctuations)
        {
            if (punctuationCategory.Key.Contains(character))
            {
                waitTime = punctuationCategory.Value;
                return true;
            }
        }

        waitTime = default;
        return false;
    }
}
