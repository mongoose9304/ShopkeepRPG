using UnityEngine;

namespace Shopkeeper {
    public class CharacterWalking : MonoBehaviour {
        public float moveSpeed;
        public Vector3 moveDirection;
        public Vector3 facingDirection;

        //Are we using rigidbody?
        //If not you can just ignore this
        Rigidbody rbComponent;

        void Start() {
            rbComponent = GetComponent<Rigidbody>();
        }

        void Update() {
            //Make the player face the directions
            //We can make it look nicer with interpolation
            //transform.rotation = Quaternion.LookRotation(facingDirection.normalized, Vector3.up);

            if (rbComponent != null) {
                rbComponent.velocity = moveDirection * moveSpeed;
            } else {
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
            }
        }
    }
}