using UnityEngine;

namespace Shopkeeper {

    [System.Serializable]
    public struct Knockback {
        [HideInInspector]
        public Vector3 direction; //normalized direction
        public float amount;
    }
}