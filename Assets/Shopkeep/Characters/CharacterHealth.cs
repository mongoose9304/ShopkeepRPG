using UnityEngine;
using UnityEngine.Events;

namespace Shopkeeper {
    public class CharacterHealth : MonoBehaviour {
        [SerializeField]
        private float _currentHealth;
        [SerializeField]
        private float _maxHealth;
        public float currentHealth
        {
            get { return _currentHealth; }
            set
            {
                _currentHealth = value;
                OnCurrHealthChange?.Invoke(_currentHealth);
            }
        }

        public float maxHealth {
            get { return _maxHealth; }
            set {
                _maxHealth = value;
                OnMaxHealthChange.Invoke(_maxHealth);
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