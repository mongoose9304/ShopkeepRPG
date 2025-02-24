using UnityEngine;

namespace Minimaps {
    public class MinimapIconSimple : MinimapIcon {
        public sealed override bool Dynamic {
            get => m_Dynamic;
        }
        public sealed override bool Rotates {
            get => m_Rotate;
        }

        public sealed override Transform Instantiate(Transform owner) {
            return Instantiate(m_Prefab, owner).transform;
        }

        private void OnEnable() {
            Minimap.GlobalCreateIcon(this);
        }
        private void OnDisable() {
            Minimap.GlobalDestroyIcon(this);
        }

        [SerializeField]
        private GameObject m_Prefab;
        [SerializeField]
        private bool m_Dynamic;
        [SerializeField]
        private bool m_Rotate;
    }
}
