using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    public Text hearts;
    public void SetHearts(int currentHealth) {
        hearts.text = GenerateHearts(currentHealth);
    }

    string GenerateHearts(int currentHealth) {
        string heartBar = "";
        for (int i = 0; i < currentHealth; i++) {
            heartBar += "❤️";
        }
        return heartBar;
    }
}
