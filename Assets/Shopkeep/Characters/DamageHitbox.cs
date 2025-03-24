using UnityEngine;

namespace Shopkeeper {
    public class DamageHitbox : MonoBehaviour {
        public Damage damage;
        private void OnTriggerEnter(Collider other) {
            CharacterHealth healthComp = other.gameObject.GetComponent<CharacterHealth>();
            if (healthComp == null) { return; }
            healthComp.TakeDamage(damage);
        }
    }
}
