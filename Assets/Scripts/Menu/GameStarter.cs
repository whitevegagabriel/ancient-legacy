using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class GameStarter : MonoBehaviour
    {
        public void StartGame() {
            SceneManager.LoadScene("LevelOneScene");
            Time.timeScale = 1f;
            PlayerStat.jumpCount = 0;
            PlayerStat.runCount = 0;
        }
    
        void OnTriggerEnter(Collider c)
        {
            if (c.CompareTag("Player"))
            {
                StartGame();
            }
        }
    }
}
