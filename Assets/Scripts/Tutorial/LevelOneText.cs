using System.Collections;
using TMPro;
using UnityEngine;

namespace Tutorial
{
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
            StartCoroutine(SetIntroText());
        }


        private IEnumerator SetIntroText()
        {
            yield return new WaitForSeconds(1);
            introText.gameObject.SetActive(true);
            textWriter.Addwriter(introText, "During the game,\n" +
                                            "talk to Friendly Knight for some hints!", 0.05f);
            yield return new WaitForSeconds(4);
            introText.gameObject.SetActive(false);
        }
    }
}
