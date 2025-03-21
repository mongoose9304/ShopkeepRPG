using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Shopkeeper
{
    public class StatusEffect : MonoBehaviour
    {
        private float duration;
        private DamageType type;

        public void SetEffect(DamageType effect)
        {
            type = effect;
        }

        // Start is called before the first frame update
        void Start()
        {
            type = DamageType.ICE;
            duration = 1.0f;
        }

        // Update is called once per frame
        void Update()
        {
            duration -= Time.deltaTime;
            if (duration <= 0.0f)
            {
                // Remove this component if the effect is expired
                Destroy(this);
            }

            switch (type) {
                case DamageType.FIRE:
                    // Damage per frame isn't great will make this a periodic tick later
                    gameObject.GetComponent<CharacterHealth>().TakeDamage(new Damage(0.1f));
                    break;
                case DamageType.ICE:
                    Stun s = new Stun();
                    s.duration = 1.0f;
                    gameObject.GetComponent<CharacterStun>().ApplyStun(s);
                    break;
                case DamageType.LIGHTNING:
                    // Find nearby enemies
                    // Check if they're close enough for lightning to chain to them
                    // Check if we still have chains available
                    // If there are still chains available, check each previous target in order, see if any of them havent' been damaged by this attack yet
                    // Apply neutral damage to any enemy hit by this attack
                    // For infinite chaining between the same 2 targets you can just apply lightning damage here instead of neutral
                    break;
            }
           
        }
    }
}