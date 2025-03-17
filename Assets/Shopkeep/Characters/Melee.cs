using UnityEngine;
namespace Shopkeeper {
    [CreateAssetMenu(fileName = "Data", menuName = "Spells/Melee")]
    public class Melee : Spell {
        GameObject meleePrefab;
        public override void Cast() {
            GameObject g = Instantiate(meleePrefab);
        }
    }
}
