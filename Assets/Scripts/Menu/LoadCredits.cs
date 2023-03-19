using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadCredits : MonoBehaviour
{
    public Text text;
    
    public void StartCredits()
    {
        SceneManager.LoadScene("CreditsMenu");
    }

    private void Start()
    {
        if (text == null) return;
        
        var credits = Resources.Load<TextAsset>("Credits");
        text.text = credits.text;
    
    }
}
