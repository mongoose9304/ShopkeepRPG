using UnityEngine;

namespace Shopkeeper 
{
    public class KnockbackHitbox : MonoBehaviour 
    {
        public Knockback effect;

        public void ApplyKnockback(CharacterKnockback c) 
        {
            Knockback finalEffect = effect;
            finalEffect.direction = transform.forward;
            c.ApplyKnockback(finalEffect);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponentInParent<CharacterKnockback>()) 
            {
                ApplyKnockback(other.GetComponentInParent<CharacterKnockback>());
            }
        }
    }
}
