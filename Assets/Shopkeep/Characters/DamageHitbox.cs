using UnityEngine;

namespace Shopkeeper {
    public class DamageHitbox : MonoBehaviour {
        public Damage damage;
        private void OnTriggerEnter(Collider other) {
            CharacterHealth healthComp = other.gameObject.GetComponent<CharacterHealth>();
            if (healthComp == null) { return; }
            Vector3 dir = (transform.position - other.transform.position).normalized;
            ShieldComponent shield = other.GetComponent<ShieldComponent>();
            if (shield != null && shield.Block(dir))
            {
                Debug.Log("blocked");
                return;
            }
            healthComp.TakeDamage(damage);
        }
    }
}
