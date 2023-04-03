using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SceneManagement
{
    public class LoadCredits : MonoBehaviour
    {
        public Text text;
    
        public void StartCredits()
        {
            SceneLoader.LoadScene(SceneName.CreditsMenu);
        }

        private void Start()
        {
            if (text == null) return;
        
            var credits = Resources.Load<TextAsset>("FinalCredits");
            text.text = credits.text;
        }
    }
}
