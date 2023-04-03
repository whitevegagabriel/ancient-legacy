using System;
using StateManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    public class GameRestarter : MonoBehaviour
    {
        public void RestartLevel()
        {
            PlayerController.health = PlayerState.SavedHealth;
            Enum.TryParse(SceneManager.GetActiveScene().name, out SceneName sceneName);
            SceneLoader.LoadScene(sceneName);
        }
        
        public void RestartGame()
        {
            PlayerController.health = 10;
            SceneLoader.LoadScene(SceneName.TutorialScene);
        }
    }
}