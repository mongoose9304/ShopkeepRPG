using Shopkeeper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper
{
    [CreateAssetMenu(fileName = "Lunge", menuName = "Spells/Enemy/Lunge")]
    public class Lunge : Spell
    {
        public float lungeForce = 20f;

        public Damage _damage;

        public override void Cast(GameObject caster)
        {
            base.Cast(caster);
            Vector3 direction = caster.transform.forward;
            caster.GetComponent<DamageHitbox>().damage = _damage;
            caster.GetComponent<Rigidbody>().AddForce(direction * lungeForce, ForceMode.VelocityChange);
        }
    }
}
