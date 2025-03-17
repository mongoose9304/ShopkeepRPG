
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Shopkeeper {
    [CreateAssetMenu(fileName = "Data", menuName = "Spells/Spell")]
    public class Spell : ScriptableObject {

        public float cooldownDuration;
        public Coroutine cooldown;

        public virtual void Cast() {
           
        }

        public IEnumerator CooldownCoroutine() {
            yield return new WaitForSeconds(cooldownDuration);
            cooldown = null;
        }
    }
}