using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper {
    public class MeleeHitbox : MonoBehaviour {
        public void StartLifeCoroutine(float duration) {
            StartCoroutine(LifeCoroutine(duration));
        }

        //apply spell
        public virtual void OnTriggerEnter(Collider other)
        {

        }
        IEnumerator LifeCoroutine(float duration) {
            yield return new WaitForSeconds(duration);
            Destroy(gameObject);
        }
    }
}
