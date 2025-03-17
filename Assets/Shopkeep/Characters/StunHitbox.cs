using UnityEngine;

namespace Shopkeeper 
{
    public class StunHitbox : MonoBehaviour 
    {
        public Stun effect;

        public void ApplyStun(CharacterStun c) 
        {
            c.ApplyStun(effect);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<CharacterStun>())
            {
                ApplyStun(other.GetComponentInParent<CharacterStun>());
            }
        }
    }
}

