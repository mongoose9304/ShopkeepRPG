using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper
{
    public class DamageProjectileComponent : ProjectileComponent
    {
        [SerializeField]
        private Damage damage;
        //apply spell
        public override void OnTriggerEnter(Collider other)
        {
            CharacterHealth health = other.GetComponent<CharacterHealth>();
            if (health)
            {
                health.TakeDamage(damage.value);
            }
        }
    }
}
