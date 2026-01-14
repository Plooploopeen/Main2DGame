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

    public bool isRunning { get; private set; }

    private readonly List<Puntuation> punctuations = new List<Puntuation>()
    {
        new Puntuation(new HashSet<char>(){'.', '!', '?', }, 0.6f),
        new Puntuation(new HashSet<char>(){',', ';', ':', }, 0.3f)
    };

    private Coroutine typingCoroutine;
    public void Run(string textToType, TMP_Text text)
    {
        typingCoroutine = StartCoroutine(TypeText(textToType, text));
    }

    public void Stop()
    {
        StopCoroutine(typingCoroutine);
        isRunning = false;
    }

    IEnumerator TypeText(string textToType, TMP_Text text)
    {
        isRunning = true;
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

        isRunning = false;
    }

    private bool IsPunctuation(char character, out float waitTime)
    {
        foreach(Puntuation punctuationCategory in punctuations)
        {
            if (punctuationCategory.Punctuations.Contains(character))
            {
                waitTime = punctuationCategory.WaitTime;
                return true;
            }
        }

        waitTime = default;
        return false;
    }

    private readonly struct Puntuation
    {
        public readonly HashSet<char> Punctuations;
        public readonly float WaitTime;

        public Puntuation(HashSet<char> punctuations, float waitTime)
        {
            Punctuations = punctuations;
            WaitTime = waitTime;
        }
    }
}
