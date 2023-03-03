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
        healthDisplay.SetHearts();
    }

    public void DecreaseHealth(int amount) {
        currentHealth = Mathf.Max(0, currentHealth - amount);
        healthDisplay.SetHearts();
    }

    public int GetCurrentHealth() {
        return currentHealth;
    }

    public int GetMaxHealth() {
        return maxHealth;
    }

    private void Update() {
        if (Input.GetKeyDown("i")) {
            IncreaseHealth(1);
        }
    }
}
