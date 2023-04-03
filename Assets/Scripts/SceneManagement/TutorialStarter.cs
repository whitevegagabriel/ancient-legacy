using SceneManagement;
using StateManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    public class TutorialStarter : MonoBehaviour
    {
        public void StartTutorial() {
            SceneLoader.LoadScene(SceneName.TutorialScene);
            Time.timeScale = 1f;
            PlayerState.JumpCount = 0;
            PlayerState.RunCount = 0;
        }
    }
}
