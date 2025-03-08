using UnityEngine;

namespace Shopkeeper {
    public class KnockbackHitbox : MonoBehaviour {
        public Knockback effect;
        
        public void ApplyKnockback(CharacterKnockback c) {
            c.ApplyKnockback(effect);
        }
    }
}
