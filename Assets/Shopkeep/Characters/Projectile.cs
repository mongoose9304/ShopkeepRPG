using UnityEngine;

namespace Shopkeeper {
    [CreateAssetMenu(fileName = "Data", menuName = "Spells/Projectile")]
    public class Projectile : Spell {
        public float projectileDuration;
        public GameObject projectilePrefab;

        public override void Cast() {
            
        }
    }
}