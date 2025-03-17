using System.Collections;
using UnityEngine;
using UnityEngine.Events;
namespace Shopkeeper {
    public class CharacterKnockback : MonoBehaviour {
        Rigidbody rbComponent;
        public UnityEvent<Knockback> OnKnockback;
        public UnityEvent<Knockback> OnKnockbackEnd;

        void Start() {
            rbComponent = GetComponent<Rigidbody>();
        }
        
        public void ApplyKnockback(Knockback k) {
            if(rbComponent == null) { return; }

            //Knockback code
            Debug.Log("Apply Force");
            Vector3 force = k.direction * k.amount;
            rbComponent.AddForce(force, ForceMode.Impulse);
            StartCoroutine(KnockbackEvents(k));
        }

        IEnumerator KnockbackEvents(Knockback k) 
        {
            OnKnockback.Invoke(k);
            yield return new WaitForSeconds(0.75f);
            OnKnockbackEnd.Invoke(k);
        }
    }
}