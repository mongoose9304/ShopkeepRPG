using UnityEngine;


namespace Shopkeeper {
    
    public class Ray : Spell {

        public GameObject rayPrefab;
        public override void Cast(GameObject caster)
        {
            GameObject g = Instantiate(rayPrefab, caster.transform.position, Quaternion.identity);

            RaycastHit hit;
            if (Physics.Raycast(caster.transform.position, caster.transform.forward, out hit))
            {
                ApplySpell(hit);
            }
        }

        public virtual void ApplySpell(RaycastHit hit)
        {
            Debug.Log(hit.transform.name);
        }

    }
}