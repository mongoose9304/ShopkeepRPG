using UnityEngine;

namespace Minimaps {
    public abstract class MinimapIcon : MonoBehaviour {
        public abstract bool Dynamic { get; }
        public abstract bool Rotates { get; }
        public abstract Transform Instantiate(Transform owner);
    }
}
