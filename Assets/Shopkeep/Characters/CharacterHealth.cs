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

        public UnityEvent<FloatWrapper> OnDamageTaken;
        public UnityEvent<float> OnCurrHealthChange;
        public UnityEvent<float> OnMaxHealthChange;


        public void TakeDamage(float damage) {
            FloatWrapper f = new FloatWrapper(damage);
            OnDamageTaken.Invoke(f);
            currentHealth -= f.value;

            if (currentHealth < 0.0000001) {
                Debug.Log("Character died.");
                Destroy(gameObject);
            }
        }
    }
}