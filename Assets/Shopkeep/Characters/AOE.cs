using Unity.VisualScripting;
using UnityEngine;

namespace Shopkeeper {
    public class AOE : Spell{
        public float radius;
        public GameObject aoePrefab;

        public override void Cast(GameObject caster)
        {
            GameObject g = Instantiate(aoePrefab, caster.transform.position, Quaternion.identity);

            RaycastHit hit;
            if (Physics.SphereCast(caster.transform.position, radius, caster.transform.forward, out hit))
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
