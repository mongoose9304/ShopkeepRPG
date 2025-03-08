using UnityEngine;
using UnityEngine.Events;


namespace Shopkeeper {
    public class ChracterStun : MonoBehaviour {
        float timeRemaining;
        Stun current;

        public UnityEvent OnStart;
        public UnityEvent OnEnd;

        public void ApplyStun(Stun s) {
            //Stun code
            if(current != null) { return; }
            current = s;
            timeRemaining = s.duration;
            Stun();
            OnStart.Invoke();
        }

        void Stun() {
            //stun code
        }

        public void Update() {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0 && current != null) {
                OnEnd.Invoke();
                current = null;
            }
        }
    }
}