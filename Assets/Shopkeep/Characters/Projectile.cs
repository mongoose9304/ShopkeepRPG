using UnityEngine;

namespace Shopkeeper {
    [CreateAssetMenu(fileName = "Data", menuName = "Spells/Projectile")]
    public class Projectile : Spell {
        public GameObject projectilePrefab;

        public override void Cast(GameObject caster) {
            //CharacterWalking walkComponent = caster.GetComponent<CharacterWalking>();
            //if(walkComponent == null) { Debug.Log("Walk Component not found"); return; }

            GameObject g = Instantiate(projectilePrefab, caster.transform.position, Quaternion.identity);
            g.GetComponent<ProjectileComponent>().Init(caster.transform.forward);
        }
    }
}