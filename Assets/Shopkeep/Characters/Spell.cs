
using System.Collections;
using UnityEngine;

namespace Shopkeeper {
    [CreateAssetMenu(fileName = "Data", menuName = "Spells/Spell")]
    public class Spell : ScriptableObject {

        public float cooldownDuration;
        public bool isCooldown = false;

        //Implement the actual cast function here;
        public virtual void Cast() {
            //Code here
            StartCooldown();
        }

        public virtual void StartCooldown() {
            isCooldown = true;
            //cooldown code
        }
    }
}