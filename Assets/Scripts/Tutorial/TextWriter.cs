using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextWriter : MonoBehaviour
{
    private TMP_Text uiText;
    private string textToWrite;
    private int characterIndex;
    private float timePerCharacter;
    private float timer;

    public void Addwriter(TMP_Text uiText, string textToWrite, float timePerCharacter)
    {
        this.uiText = uiText;
        this.textToWrite = textToWrite;
        this.timePerCharacter = timePerCharacter;
        characterIndex = 0;
    }

    public void Reset()
    {
        uiText = null;
        textToWrite = null;
        characterIndex = 0;
        timePerCharacter = 0f;
        timer = 0f;
    }

    private void Update()
    {
        if (uiText == null) return;
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer += timePerCharacter;
            characterIndex++;
            uiText.text = textToWrite.Substring(0, characterIndex);
        }
        if (characterIndex >= textToWrite.Length) {
            uiText = null;
        }
    }
}
