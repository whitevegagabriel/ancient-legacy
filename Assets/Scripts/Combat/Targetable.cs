using UnityEngine;

namespace Combat
{
    public class Targetable : MonoBehaviour
    {
        private int health;
        private int _maxHealth;
        private HealthUI _healthDisplay;
        private PlayerController player;

        void Start()
        {
            _healthDisplay = GetComponent<HealthUI>();
            if (_healthDisplay != null) {
                _healthDisplay.SetHearts(health, _maxHealth);
            }
            player = GetComponent<PlayerController>();
        }

        public void OnHit(int damage)
        {
            health -= damage;
            if (player != null)
            {
                PlayerController.health -= 1;
            }
            if (_healthDisplay != null) {
                _healthDisplay.SetHearts(health, _maxHealth);
            }
        }
        
        public void InitHealth(int newHealth, int maxHealth)
        {
            health = newHealth;
            _maxHealth = maxHealth;
        }
        
        public int GetHealth()
        {
            return health;
        }
    }
}