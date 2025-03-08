using UnityEngine;
using UnityEngine.Events;

namespace Shopkeeper {
    public class CharacterHealth : MonoBehaviour {
        public float currentHealth {
            get { return currentHealth; }
            set {
                currentHealth = value;
                OnCurrHealthChange.Invoke(currentHealth);
            }

        }

        public float maxHealth {
            get { return currentHealth; }
            set {
                currentHealth = value;
                OnMaxHealthChange.Invoke(currentHealth);
            }
        }

        public UnityEvent<float> OnDamageTaken;
        public UnityEvent<float> OnCurrHealthChange;
        public UnityEvent<float> OnMaxHealthChange;


        public void TakeDamage(float damage) {
            OnDamageTaken.Invoke(damage);
            currentHealth -= damage;
        }
    }
}