using UnityEngine;

namespace Shopkeeper {
    public class KnockbackHitbox : MonoBehaviour {
        public Knockback effect;
        
        private void OnTriggerEnter(Collider other) {
            CharacterKnockback knockbackComp = other.gameObject.GetComponent<CharacterKnockback>();
            if(knockbackComp == null) { return; }
            knockbackComp.ApplyKnockback(effect);
        }
    }
}
