using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public class HealthUI : MonoBehaviour
    {
        public Text hearts;
        public void SetHearts(int currentHealth, int maxHealth) {
            hearts.text = GenerateHearts(currentHealth, maxHealth);
        }

        string GenerateHearts(int currentHealth, int maxHealth) {
            string heartBar = "";
            for (int i = 0; i < currentHealth; i++) {
                heartBar += "❤️";
            }
            for (int i = 0; i < maxHealth - currentHealth; i++) {
                heartBar += "X";
            }
            return heartBar;
        }
    }
}
