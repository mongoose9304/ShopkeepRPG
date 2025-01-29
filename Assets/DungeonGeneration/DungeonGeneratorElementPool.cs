using UnityEngine;

namespace Dungeons {
    public struct DungeonGeneratorElementPool {
        public bool IsEmpty {
            get => m_Count == 0;
        }

        public DungeonGeneratorElementPool(int capacity) {
            m_Anchors = new InternalAnchor[capacity];
            m_Weights = new float[capacity];
            m_Capacity = capacity;
            m_Weight = 0;
            m_Count = 0;
        }

        public void Reset() {
            m_Count = 0;
            m_Weight = 0;
        }
        public bool Append(DungeonLayoutElement element, int index, float weight) {
            if (m_Count < m_Capacity) {
                m_Anchors[m_Count].element = element;
                m_Anchors[m_Count].index = index;
                m_Weights[m_Count] = weight;
                m_Weight += weight;
                ++m_Count;
                return true;
            }
            return false;
        }
        public readonly bool Sample(out DungeonLayoutElement element, out int index) {
            if (m_Count > 0) {
                var random = Random.Range(0, m_Weight);
                var cursor = 0.0f;
                var count = m_Count;
                var weights = m_Weights;
                for (int i = 0; i < count; ++i) {
                    cursor += weights[i];
                    if (cursor >= random) {
                        element = m_Anchors[i].element;
                        index = m_Anchors[i].index;
                        return true;
                    }
                }
            }
            element = default;
            index = default;
            return false;
        }
        
        private readonly InternalAnchor[] m_Anchors;
        private readonly float[] m_Weights;
        private int m_Capacity;
        private float m_Weight;
        private int m_Count;

        private struct InternalAnchor {
            public DungeonLayoutElement element;
            public Vector2 position;
            public int index;
        }
    }
}
