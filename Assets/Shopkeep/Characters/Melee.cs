using UnityEngine;
namespace Shopkeeper {
    [CreateAssetMenu(fileName = "Data", menuName = "Spells/Melee")]
    public class Melee : Spell {

        public float meleeDuration;
        public GameObject meleePrefab;

        public override void Cast(GameObject caster) {
           if(meleePrefab == null) { return; }
            GameObject mb = Instantiate(meleePrefab, caster.transform.position, caster.transform.rotation);
            mb.GetComponent<MeleeHitbox>().StartLifeCoroutine(meleeDuration);
        }

    }
}
