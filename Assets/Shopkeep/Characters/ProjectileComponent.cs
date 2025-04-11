using System.Collections;
using UnityEngine;

namespace Shopkeeper {
    public class ProjectileComponent : MonoBehaviour {
        public float speed;
        public float duration;

        Rigidbody rbComponent;

        private void Awake() {
            rbComponent = GetComponent<Rigidbody>();
        }

        //apply spell
        public virtual void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.name);
        }

        public void Init(Vector3 dir) {
            rbComponent.velocity = dir.normalized * speed;
            StartCoroutine(LifeCoroutine());
        }

        IEnumerator LifeCoroutine() {
            yield return new WaitForSeconds(duration);
            Destroy(gameObject);
        }
    }
}
