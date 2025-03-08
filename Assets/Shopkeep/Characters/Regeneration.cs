using UnityEngine;
using UnityEngine.Events;

namespace Shopkeeper {
    public class Regeneration : MonoBehaviour {
        public UnityEvent<Stat> OnRegenChange;
        public Shopkeeper.Stat regen {
            get { return regen; }
            set {
                regen = value;
                OnRegenChange.Invoke(regen);
            }
        }

        void Regen() {
            //Regen code here
        }

        void Update() {
            Regen();
        }
    }
}