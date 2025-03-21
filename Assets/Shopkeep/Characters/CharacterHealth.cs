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


        public void TakeDamage(Damage damage) {
            OnDamageTaken.Invoke(damage.amount);
            currentHealth -= damage.amount;

            switch (damage.type)
            {
                case DamageType.NEUTRAL:
                    // Nothing
                    break;
                case DamageType.FIRE:
                    gameObject.AddComponent<StatusEffect>().SetEffect(DamageType.FIRE);
                    break;
                case DamageType.ICE:
                    gameObject.AddComponent<StatusEffect>().SetEffect(DamageType.ICE);
                    break;
                case DamageType.LIGHTNING:
                    gameObject.AddComponent<StatusEffect>().SetEffect(DamageType.LIGHTNING);
                    break;
            }
        }
    }
}