using UnityEngine;

namespace Shopkeeper {
    public class StunHitbox : MonoBehaviour {
        public Stun effect;

        private void OnTriggerEnter(Collider other) {
            CharacterStun stunComp = other.gameObject.GetComponent<CharacterStun>();
            if (stunComp == null) { return; }
            stunComp.ApplyStun(effect);
        }
    }
}

