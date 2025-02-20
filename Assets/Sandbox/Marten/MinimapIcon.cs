using UnityEngine;

namespace Minimaps {
    public class MinimapIcon : MonoBehaviour {
        public GameObject Prefab {
            get => m_Prefab;
        }
        public bool Dynamic {
            get => m_Dynamic;
        }
        public bool Rotate {
            get => m_Rotate;
        }

        private void OnEnable() {
            Minimap.GlobalCreateIcon(transform, m_Prefab, m_Dynamic, m_Rotate);
        }
        private void OnDisable() {
            Minimap.GlobalDestroyIcon(transform);
        }
        
        [SerializeField]
        private GameObject m_Prefab;
        [SerializeField]
        private bool m_Dynamic;
        [SerializeField]
        private bool m_Rotate;
    }
}
