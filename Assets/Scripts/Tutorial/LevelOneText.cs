using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelOneText : MonoBehaviour
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
        textWriter.Addwriter(introText, "During the game!\n" +
            "You can always talk to Friendly Knight for some hints", 0.08f);
        yield return new WaitForSeconds(8);
        introText.gameObject.SetActive(false);
    }
}
