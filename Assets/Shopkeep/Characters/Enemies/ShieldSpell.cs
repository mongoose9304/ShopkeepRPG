using System.Collections;
using UnityEngine;

namespace Shopkeeper
{
    [CreateAssetMenu(fileName = "Shield", menuName = "Spells/Enemy/Shield")]
    public class Shield : Spell
    {
        public float effectDuration = 3f;
        public float forwardAngle = 120f; // degrees of frontal protection

        public override void Cast(GameObject caster)
        {
            base.Cast(caster);

            ShieldComponent shield = caster.GetComponent<ShieldComponent>();
            if (shield == null)
            {
                shield = caster.AddComponent<ShieldComponent>();
            }

            shield.Activate(effectDuration, forwardAngle);
        }
    }
}