using UnityEngine;

namespace Shopkeeper {
    public class DamageHitbox : MonoBehaviour {
        public Damage amount;

        private void OnTriggerEnter(Collider other) {
            CharacterHealth healthComp = other.gameObject.GetComponent<CharacterHealth>();
            if (healthComp == null) { return; }
            healthComp.TakeDamage(amount);
        }
    }
}
