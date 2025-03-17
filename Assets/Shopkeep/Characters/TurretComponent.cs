using Shopkeeper;
using UnityEngine;

namespace Shopkeeper {
    public class TurretComponent : MonoBehaviour {
        float duration;
        Spell spell;

        public void Update() {
            spell.Cast();
        }

        public void OnDeath() { }
    }
}
