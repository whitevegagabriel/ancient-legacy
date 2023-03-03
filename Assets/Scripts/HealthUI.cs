using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthUI : MonoBehaviour
{
    PlayerHealth playerHealth;
    public Text hearts;

    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        if (playerHealth == null) {
            Debug.Log("Player not found");
        }
        SetHearts();
    }

    public void SetHearts() {
        hearts.text = GenerateHearts();
    }

    string GenerateHearts() {
        string heartBar = "";
        for (int i = 0; i < playerHealth.GetCurrentHealth(); i++) {
            heartBar += "❤️";
        }
        for (int i = 0; i < playerHealth.GetMaxHealth() - playerHealth.GetCurrentHealth(); i++) {
            heartBar += "X";
        }
        return heartBar;
    }
}
