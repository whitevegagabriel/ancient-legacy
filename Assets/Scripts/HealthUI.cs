using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthUI : MonoBehaviour
{
    PlayerHealth playerHealth;
    public Text hearts;
    string[] Hearts;    

    void Start()
    {
        Hearts = new string[11];
        Hearts[0] = "X X X X X X X X X X";
        Hearts[1] = "❤️ X X X X X X X X X";
        Hearts[2] = "❤️❤️ X X X X X X X X";
        Hearts[3] = "❤️❤️❤️X X X X X X X";
        Hearts[4] = "❤️❤️❤️❤️X X X X X X";
        Hearts[5] = "❤️❤️❤️❤️❤️X X X X X";
        Hearts[6] = "❤️❤️❤️❤️❤️❤️ X X X X";
        Hearts[7] = "❤️❤️❤️❤️❤️❤️❤️ X X X";
        Hearts[8] = "❤️❤️❤️❤️❤️❤️❤️❤️ X X";
        Hearts[9] = "❤️❤️❤️❤️❤️❤️❤️❤️❤️ X";
        Hearts[10] = "❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️";

        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        if (playerHealth == null) {
            Debug.Log("Player not found");
        }
        hearts.text = Hearts[playerHealth.GetHealth()];
    }

    public void SetHearts(int health) {
        hearts.text = Hearts[playerHealth.GetHealth()];
    }
}
