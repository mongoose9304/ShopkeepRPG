using UnityEngine;
using UnityEngine.Events;

namespace Shopkeeper {
    public sealed class ManagerGlobal : MonoBehaviour {
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
            if (!s_Manager) {
                s_Manager = gameObject;
                DontDestroyOnLoad(gameObject);
            }
            else {
                Destroy(gameObject);
            }
        }
        private void Start() {
            m_EventStart.Invoke();
        }
        
        [SerializeField]
        private UnityEvent m_EventStart;
        private static GameObject s_Manager;
    }
}
