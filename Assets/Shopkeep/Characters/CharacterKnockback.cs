
using UnityEngine;
using UnityEngine.Events;
namespace Shopkeeper {
    public class CharacterKnockback : MonoBehaviour {
        Rigidbody rbComponent;
        public UnityEvent<Knockback> OnKnockback;

        void Start() {
            rbComponent = GetComponent<Rigidbody>();
        }
        
        public void ApplyKnockback(Knockback k) {
            if(rbComponent == null) { return; }

            //Knockback code
            OnKnockback.Invoke(k);

        }
    }
}