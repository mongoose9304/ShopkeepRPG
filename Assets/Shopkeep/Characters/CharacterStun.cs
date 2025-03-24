using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Shopkeeper 
{
    public class CharacterStun : MonoBehaviour 
    {
        private float timeRemaining = 0;
        bool stunned = false;

        public UnityEvent OnStart;
        public UnityEvent OnEnd;
        public void ApplyStun(Stun s) 
        {
            timeRemaining = s.duration;
            stunned = true;
            OnStart.Invoke();
        }

        public void Update() 
        {
            if (stunned) 
            {
                timeRemaining -= Time.deltaTime;
                if (timeRemaining < 0.000001)
                {
                    OnEnd.Invoke();
                    stunned = false;
                }
            }
            
        }
    }
}