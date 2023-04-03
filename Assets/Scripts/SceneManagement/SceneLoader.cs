using System;
using StateManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    public static class SceneLoader
    {
        public static void LoadScene(SceneName sceneName)
        {
            SceneManager.LoadScene(sceneName.ToString());
            Time.timeScale = 1f;
            switch (sceneName)
            {
                case SceneName.StartMenu:
                    break;
                case SceneName.LevelOneScene:
                    PlayerState.JumpCount = 0;
                    PlayerState.RunCount = 0;
                    PlayerState.SavedHealth = 10;
                    break;
                case SceneName.CreditsMenu:
                    break;
                case SceneName.TutorialScene:
                    PlayerState.JumpCount = 0;
                    PlayerState.RunCount = 0;
                    PlayerState.SavedHealth = 10;
                    break;
                case SceneName.LevelOneBossScene:
                    PlayerState.JumpCount = 3;
                    PlayerState.RunCount = 3;
                    PlayerState.SavedHealth = PlayerController.health;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sceneName), sceneName, null);
            }
        }
    }
}