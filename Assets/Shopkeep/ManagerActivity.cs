using UnityEngine;
using UnityEngine.Events;

namespace Shopkeeper {
    public sealed class ManagerActivity : MonoBehaviour {
        /// <summary>
        /// Returns the manager of the specified type.
        /// </summary>
        /// <typeparam name="TManager">The manager type.</typeparam>
        /// <returns>The manager of the specified type.</returns>
        public static TManager GetManager<TManager>() {
            return s_Manager.GetComponent<TManager>();
        }
        /// <summary>
        /// Returns the managers of the specified type.
        /// </summary>
        /// <typeparam name="TManager">The manager type.</typeparam>
        /// <returns>The managers of the specified type.</returns>
        public static TManager[] GetManagers<TManager>() {
            return s_Manager.GetComponents<TManager>();
        }

        private void Awake() {
            s_Manager = gameObject;
        }
        private void Start() {
            m_EventStart.Invoke();
        }
        private void OnDestroy() {
            if (s_Manager == gameObject)
                s_Manager = null;
        }
        
        [SerializeField]
        private UnityEvent m_EventStart;
        private static GameObject s_Manager;
    }
}
