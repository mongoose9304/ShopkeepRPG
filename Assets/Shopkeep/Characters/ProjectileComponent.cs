using UnityEngine;

namespace Shopkeeper {
    public class ProjectileComponent : MonoBehaviour {
        public float duration;
        public virtual void OnHit() { }
    }
}
