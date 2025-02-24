using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

namespace Minimaps {
    public class MinimapIconDungeonLayout : MinimapIcon {
        public override bool Dynamic => false;
        public override bool Rotates => true;

        public override Transform Instantiate(Transform owner) {
            var go = new GameObject();
            var rt = go.AddComponent<RectTransform>();
            rt.anchorMin = Vector3.one * 0.5f;
            rt.anchorMax = Vector3.one * 0.5f;
            rt.SetParent(owner);
            rt.localScale = Vector3.one;
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1.0f);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1.0f);

            if (m_Layout) {
                foreach (ref readonly var rect in m_Layout.Bounds) {
                    var center = rect.center;
                    var size = rect.size;
                    var goc = Instantiate(m_Prefab, rt);
                    var rtc = goc.GetComponent<RectTransform>();
                    goc.GetComponent<Image>().color = m_Discovered ? Color.white : Color.black;
                    rtc.anchorMin = Vector3.one * 0.5f;
                    rtc.anchorMax = Vector3.one * 0.5f;
                    rtc.SetParent(rt);
                    rtc.localScale = Vector3.one;
                    rtc.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y - 1.0f);
                    rtc.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x - 1.0f);
                    rtc.SetLocalPositionAndRotation(new(center.x, center.y, 0.0f), Quaternion.identity);
                }
            }

            return go.transform;
        }

        public void Discover() {
            if (!m_Discovered) {
                m_Discovered = true;
                if (enabled) {
                    Minimap.GlobalCreateIcon(this);
                }
            }
        }
        public void Undiscover() {
            if (m_Discovered) {
                m_Discovered = false;
                if (enabled) {
                    Minimap.GlobalCreateIcon(this);
                }
            }
        }

        private void OnEnable() {
            Minimap.GlobalCreateIcon(this);
        }
        private void OnDisable() {
            Minimap.GlobalDestroyIcon(this);
        }

        [SerializeField]
        private bool m_Discovered;
        [SerializeField]
        private GameObject m_Prefab;
        [SerializeField]
        private Dungeons.DungeonLayoutElement m_Layout;
    }
}
