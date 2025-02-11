using System;
using UnityEngine;

namespace Dungeons {
    /// <summary>
    /// Defines the layout for a dungeon element.
    /// </summary>
    [CreateAssetMenu(menuName = "Dungeons/DungeonElement", order = 2)]
    public class DungeonLayoutElement : DungeonLayoutElementProvider {
        /// <summary>
        /// The prefab that will be instantiated when this element is placed into a dungeon.
        /// </summary>
        public GameObject Prefab {
            get => Resources.Load<GameObject>(m_Prefab);
        }
        /// <summary>
        /// The bounds used when checking for collisions between this element and the dungeon.
        /// </summary>
        public ReadOnlySpan<Rect> Bounds {
            get => m_Bounds;
        }
        /// <summary>
        /// The anchors used when connecting this element with other elements in the dungeon.
        /// </summary>
        public ReadOnlySpan<DungeonLayout.Anchor> Anchors {
            get => m_Anchors;
        }
        /// <summary>
        /// The conditions that must be true for this element to be generated in the dungeon.
        /// </summary>
        public ReadOnlySpan<DungeonGenerator.Condition> Conditions {
            get => m_Conditions;
        }
        /// <summary>
        /// The operations that will be executed when this element is generated in the dungeon.
        /// </summary>
        public ReadOnlySpan<DungeonGenerator.Operation> Operations {
            get => m_Operations;
        }
        
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
            //push valid anchors.
            var anchors = m_Anchors;
            var anchorsLength = anchors.Length;
            for (int i = 0; i < anchorsLength; ++i) {
                ref readonly var current = ref anchors[i];
                if (!DungeonLayout.AnchorsAreCompatible(current, anchor))
                    continue;
                var offset = anchor.offset - current.offset;
                if (dungeon.Collision.Intersects(m_Bounds, offset))
                    continue;
                dungeon.Potentials.Append(this, i, weight);
            }
        }
        
        [SerializeField, Tooltip("The prefab that will be instantiated when this element is placed into a dungeon. Dungeon element prefabs must be in any 'Resources/' folder. This variable contains the path relative to any 'Resources/' folder.")]
        private string m_Prefab;
        [SerializeField, Tooltip("The bounds used when checking for collisions between this element and the dungeon.")]
        private Rect[] m_Bounds;
        [SerializeField, Tooltip("The anchors used when connecting this element with other elements in the dungeon.")]
        private DungeonLayout.Anchor[] m_Anchors;
        [SerializeField, Tooltip("The conditions that must be true for this element to be generated in the dungeon.")]
        private DungeonGenerator.Condition[] m_Conditions;
        [SerializeField, Tooltip("The operations that will be executed when this element is generated in the dungeon.")]
        private DungeonGenerator.Operation[] m_Operations;
    }
}
