using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    public void StartGame() {
        SceneManager.LoadScene("LevelOneScene");
        Time.timeScale = 1f;
        PlayerStat.jumpCount = 0;
        PlayerStat.runCount = 0;
    }
}
