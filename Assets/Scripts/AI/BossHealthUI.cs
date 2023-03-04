using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    public Text hearts;
    AI.BossAI bossAI;

    void Start()
    {
        bossAI = GameObject.FindGameObjectWithTag("Boss").GetComponent<AI.BossAI>();
        if (bossAI == null) {
            Debug.Log("BossAI not found");
        }
        SetHearts();
    }

    public void SetHearts() {
        hearts.text = GenerateHearts();
    }

    string GenerateHearts() {
        string heartBar = "";
        for (int i = 0; i < bossAI.GetHealth(); i++) {
            heartBar += "❤️";
        }
        return heartBar;
    }
}
