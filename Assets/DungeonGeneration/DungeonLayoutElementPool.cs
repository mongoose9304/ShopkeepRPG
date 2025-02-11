using System;
using UnityEngine;

namespace Dungeons {
    /// <summary>
    /// Provides possible elements to the dungeon generator from a pool of elements.
    /// </summary>
    [CreateAssetMenu(menuName = "Dungeons/DungeonElementPool", order = 3)]
    public class DungeonLayoutElementPool : DungeonLayoutElementProvider {
        public override void ProvideLayoutElements(DungeonGenerator dungeon, in DungeonLayout.AnchorConcrete anchor, float weight) {
            //validate conditions defined by self.
            var conditions = m_Conditions;
            var conditionsLength = conditions.Length;
            for (int i = 0; i < conditionsLength; ++i)
                if (!dungeon.CompareCounter(conditions[i].Name, conditions[i].Value, conditions[i].Mode))
                    return;
            //validate conditions defined by dungeon layout.
            if (dungeon.Constraints.TryGetValue(this, out conditions)) {
                conditionsLength = conditions.Length;
                for (int i = 0; i < conditionsLength; ++i)
                    if (!dungeon.CompareCounter(conditions[i].Name, conditions[i].Value, conditions[i].Mode))
                        return;
            }
            //push valid elements.
            var elements = m_Elements;
            var elementsLength = elements.Length;
            for (int i = 0; i < elementsLength; ++i) {
                var current = elements[i];
                if (current.m_Element)
                    current.m_Element.ProvideLayoutElements(dungeon, anchor, weight * current.m_Weight);
                else
                    dungeon.Potentials.Append(null, i, weight * current.m_Weight);
            }
        }
        
        [SerializeField, Tooltip("The elements that are included in this pool.")]
        private Element[] m_Elements;
        [SerializeField, Tooltip("The conditions that must be true of elements from this pool to be included in the dungeon.")]
        private DungeonGenerator.Condition[] m_Conditions;
        [Serializable]
        private struct Element {
            [Tooltip("The weight (probability) that this element will be selected from the pool.")]
            public float m_Weight;
            [Tooltip("The element.")]
            public DungeonLayoutElementProvider m_Element;
        }
    }
}
