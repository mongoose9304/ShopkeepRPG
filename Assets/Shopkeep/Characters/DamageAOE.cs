using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper
{
    [CreateAssetMenu(fileName = "DamageAOE", menuName = "Spells/DamageAOE")]
    public class DamageAOE : AOE
    {
        [SerializeField]
        private Damage damage;
        public override void ApplySpell(RaycastHit hit)
        {
            CharacterHealth health = hit.rigidbody.GetComponent<CharacterHealth>();
            if (health) 
            {
                health.TakeDamage(damage.value);
            }
        }
    }
}
