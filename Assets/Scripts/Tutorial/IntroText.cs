using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroText : MonoBehaviour
{
    public TMP_Text introText;

    [SerializeField] private TextWriter textWriter;

    private void Awake()
    {
        introText.gameObject.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(setIntroText());
    }


    IEnumerator setIntroText()
    {
        yield return new WaitForSeconds(1);
        introText.gameObject.SetActive(true);
        textWriter.Addwriter(introText, "Welcome to AncientLegacy!\n" +
            "Please proceed and talk to Friendly Knight!", 0.05f);
        yield return new WaitForSeconds(4);
        introText.gameObject.SetActive(false);
    }
}
