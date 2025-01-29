using UnityEngine;

namespace Dungeons {
    public class DungeonLayoutElementVisualizer : MonoBehaviour {
        private void OnDrawGizmos() {
            if (!m_Element)
                return;

            Random.InitState(m_Element.GetHashCode());
            Gizmos.color = Random.ColorHSV(0.0f, 1.0f, 0.5f, 1.0f, 0.5f, 1.0f);
            foreach (var bound in m_Element.Bounds) {
                var size = bound.size;
                var offset = bound.center;
                var sizeXZ = new Vector3(size.x, 3.0f, size.y);
                var offsetXZ = new Vector3(offset.x, sizeXZ.y * 0.5f, offset.y);
                Gizmos.DrawWireCube(transform.position + offsetXZ, sizeXZ);
            }

            for (int i = 0; i < m_Element.Anchors.Length; ++i) {
                var anchor = m_Element.Anchors[i];
                Random.InitState(anchor.name.GetHashCode());
                Gizmos.color = Random.ColorHSV(0.0f, 1.0f, 0.5f, 1.0f, 0.5f, 1.0f);

                var offset = anchor.offset;
                var offsetXZ = new Vector3(offset.x, 0.0f, offset.y);
                var center = transform.position + offsetXZ;
                var direction = GetAnchorDirection(anchor.mode);
                Gizmos.DrawSphere(center, 0.2f);
                Gizmos.DrawLine(center, center + direction * 0.5f);
#if UNITY_EDITOR
                UnityEditor.Handles.Label(center + Vector3.up * 0.5f, string.Format("{0}:{1}", i, anchor.name));
#endif
            }
        }

        [SerializeField]
        private DungeonLayoutElement m_Element;

        private static Vector3 GetAnchorDirection(DungeonLayout.AnchorMode mode) {
            switch (mode) {
                case DungeonLayout.AnchorMode.NegativeX:
                    return Vector3.left;
                case DungeonLayout.AnchorMode.NegativeZ:
                    return Vector3.back;
                case DungeonLayout.AnchorMode.PositiveX:
                    return Vector3.right;
                case DungeonLayout.AnchorMode.PositiveZ:
                    return Vector3.forward;
                case DungeonLayout.AnchorMode.Parent:
                    return Vector3.down;
                case DungeonLayout.AnchorMode.Child:
                    return Vector3.up;
            }
            return Vector3.zero;
        }
    }
}
