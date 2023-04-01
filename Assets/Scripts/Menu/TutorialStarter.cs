using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class TutorialStarter : MonoBehaviour
    {
        public void StartTutorial() {
            SceneManager.LoadScene("TutorialScene");
            Time.timeScale = 1f;
        }
    }
}
