using Shopkeeper;
using System.Drawing.Drawing2D;
using UnityEngine;

namespace Shopkeeper {
    public class TurretComponent : MonoBehaviour {
        float timeRemaining;
        [SerializeField]
        Spell spell;
        [SerializeField]
        SpellComponent spellComponent;

        private void Start()
        {
            timeRemaining = 15f;
        }

        public void Update() {
            spellComponent.CastSpell(spell);
            
            timeRemaining -= Time.deltaTime;
            if (timeRemaining < 0.000001)
            {
                OnDeath();
            }
        }
        public void OnDeath() 
        {
            Destroy(gameObject);
        }
    }
}
