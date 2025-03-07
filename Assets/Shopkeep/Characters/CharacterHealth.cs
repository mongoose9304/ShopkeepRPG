using UnityEngine;
using UnityEngine.Events;

public class CharacterHealth : MonoBehaviour
{
    public float currentHealth {
        get { return currentHealth; }
        set { 
            OnCurrHealthChange.Invoke(currentHealth);
            currentHealth = value;
        }

    }
    public float maxHealth {
        get { return currentHealth; }
        set { 
            OnMaxHealthChange.Invoke(currentHealth);
            currentHealth = value; 
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
