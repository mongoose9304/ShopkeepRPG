using System.Collections.Generic;
using UnityEngine;

namespace Minimaps {
    public class Minimap : MonoBehaviour {
        public static void GlobalCreateIcon(Transform owner, GameObject prefab, bool dynamic, bool rotate) {
            foreach (var minimap in s_Minimaps)
                minimap.CreateIcon(owner, prefab, dynamic, rotate);
        }
        public static void GlobalDestroyIcon(Transform owner) {
            foreach (var minimap in s_Minimaps)
                minimap.DestroyIcon(owner);
        }

        public void CreateIcon(Transform owner, GameObject prefab, bool dynamic, bool rotate) {
            DestroyIcon(owner);
            var icon = Instantiate(prefab, transform).transform;
            var position = owner.localPosition;
            position.y = position.z;
            position.z = 0.0f;
            if (rotate)
                icon.SetLocalPositionAndRotation(position, Quaternion.AngleAxis(-owner.eulerAngles.y, Vector3.forward));
            else
                icon.SetLocalPositionAndRotation(position, Quaternion.Inverse(transform.rotation));
            if (dynamic || !rotate)
                m_IconsDynamic.Add(owner, (icon, rotate));
            else
                m_Icons.Add(owner, icon);
        }
        public void DestroyIcon(Transform owner) {
            if (m_Icons.TryGetValue(owner, out var icon)) {
                Destroy(icon.gameObject);
                m_Icons.Remove(owner);
            }
            else if (m_IconsDynamic.TryGetValue(owner, out var pair)) {
                Destroy(pair.Item1.gameObject);
                m_IconsDynamic.Remove(owner);
            }
        }

        private void OnEnable() {
            s_Minimaps.Add(this);
            foreach (var icon in FindObjectsOfType<MinimapIcon>())
                CreateIcon(icon.transform, icon.Prefab, icon.Dynamic, icon.Rotate);
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
