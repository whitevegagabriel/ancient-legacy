using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    int currentHealth;
    HealthUI healthDisplay;
    void Start()
    {
        currentHealth = maxHealth;  
        healthDisplay = GameObject.FindGameObjectWithTag("Health Display").GetComponent<HealthUI>();
    }

    public void IncreaseHealth(int amount) {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        healthDisplay.SetHearts(currentHealth);
    }

    public void DecreaseHealth(int amount) {
        currentHealth = Mathf.Max(0, currentHealth - amount);
        healthDisplay.SetHearts(currentHealth);
    }

    public int GetHealth() {
        return currentHealth;
    }
}
