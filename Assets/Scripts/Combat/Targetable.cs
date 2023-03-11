using UnityEngine;

namespace Combat
{
    public class Targetable : MonoBehaviour
    {
        private int health;
        private int _maxHealth;
        private HealthUI _healthDisplay;

        void Start()
        {
            _healthDisplay = GetComponent<HealthUI>();
            _healthDisplay.SetHearts(health, _maxHealth);
        }

        public void OnHit(int damage)
        {
            health -= damage;
            if (_healthDisplay != null) {
                _healthDisplay.SetHearts(health, _maxHealth);
            }
        }
        
        public void InitHealth(int newHealth)
        {
            health = newHealth;
            _maxHealth = health;
        }
        
        public int GetHealth()
        {
            return health;
        }
    }
}