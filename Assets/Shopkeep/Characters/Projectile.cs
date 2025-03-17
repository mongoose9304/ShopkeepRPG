using UnityEngine;

namespace Shopkeeper {
    [CreateAssetMenu(fileName = "Data", menuName = "Spells/Projectile")]
    public class Projectile : Spell {
        public GameObject projectilePrefab;
    }
}