using System.Collections.Generic;
using UnityEngine;

namespace Minimaps {
    public class Minimap : MonoBehaviour {
        public static void GlobalCreateIcon(MinimapIcon owner) {
            foreach (var minimap in s_Minimaps)
                minimap.CreateIcon(owner);
        }
        public static void GlobalDestroyIcon(MinimapIcon owner) {
            foreach (var minimap in s_Minimaps)
                minimap.DestroyIcon(owner);
        }

        public void CreateIcon(MinimapIcon owner) {
            DestroyIcon(owner);
            var ownerTransform = owner.transform;
            var ownerPosition = ownerTransform.position;
            var icon = owner.Instantiate(transform);
            ownerPosition.y = ownerPosition.z;
            ownerPosition.z = 0.0f;
            if (owner.Rotates)
                icon.SetLocalPositionAndRotation(ownerPosition, Quaternion.AngleAxis(-ownerTransform.eulerAngles.y, Vector3.forward));
            else
                icon.SetLocalPositionAndRotation(ownerPosition, Quaternion.Inverse(transform.rotation));
            if (owner.Dynamic || !owner.Rotates)
                m_IconsDynamic.Add(ownerTransform, (icon, owner.Rotates));
            else
                m_Icons.Add(ownerTransform, icon);
        }
        public void DestroyIcon(MinimapIcon owner) {
            var ownerTransform = owner.transform;
            if (m_Icons.TryGetValue(ownerTransform, out var icon)) {
                Destroy(icon.gameObject);
                m_Icons.Remove(ownerTransform);
            }
            else if (m_IconsDynamic.TryGetValue(ownerTransform, out var pair)) {
                Destroy(pair.Item1.gameObject);
                m_IconsDynamic.Remove(ownerTransform);
            }
        }

        private void OnEnable() {
            s_Minimaps.Add(this);
            foreach (var icon in FindObjectsOfType<MinimapIcon>())
                CreateIcon(icon);
        }
        private void Update() {
            //minimaps follow targets.
            if (m_Target) {
                var position = -m_Target.position;
                position.y = position.z;
                position.z = 0.0f;
                var rotation = Quaternion.AngleAxis(m_Target.eulerAngles.y, Vector3.forward);
                transform.SetLocalPositionAndRotation(rotation * position, rotation);
            }
            //dynamic icons follow owners.
            foreach (var (owner, (icon, rotate)) in m_IconsDynamic) {
                var position = owner.localPosition;
                position.y = position.z;
                position.z = 0.0f;
                if (rotate)
                    icon.SetLocalPositionAndRotation(position, Quaternion.AngleAxis(-owner.eulerAngles.y, Vector3.forward));
                else
                    icon.SetLocalPositionAndRotation(position, Quaternion.Inverse(transform.rotation));
            }
        }
        private void OnDisable() {
            s_Minimaps.Remove(this);
            foreach (var (_, (icon, _)) in m_IconsDynamic)
                Destroy(icon.gameObject);
            foreach (var (_, icon) in m_Icons)
                Destroy(icon.gameObject);
            m_IconsDynamic.Clear();
            m_Icons.Clear();
        }
        
        [SerializeField]
        private Transform m_Target;
        private Dictionary<Transform, Transform> m_Icons = new();
        private Dictionary<Transform, (Transform, bool)> m_IconsDynamic = new();
        private static readonly List<Minimap> s_Minimaps = new();
    }
}
