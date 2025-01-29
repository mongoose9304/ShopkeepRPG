using System;
using UnityEngine;

namespace Dungeons {
    /// <summary>
    /// Defines the layout for a dungeon.
    /// </summary>
    [CreateAssetMenu(menuName = "Dungeons/Dungeon", order = 1)]
    public class DungeonLayout : ScriptableObject {
        /// <summary>
        /// The root room.
        /// </summary>
        public DungeonLayoutElementProvider Root {
            get => m_Root;
        }
        /// <summary>
        /// The maximum room generation count. Zero indicites no limit.
        /// </summary>
        public int MaximumRooms {
            get => m_MaximumRooms;
        }
        /// <summary>
        /// The maximum room generation depth. Zero indicates no limit.
        /// </summary>
        public int MaximumDepth {
            get => m_MaximumDepth;
        }

        /// <summary>
        /// Returns true if the anchors can attach to eachother.
        /// </summary>
        /// <param name="first">The first anchor.</param>
        /// <param name="other">The second anchor.</param>
        /// <returns>True if the anchors can attach to eachother.</returns>
        public static bool AnchorsAreCompatible(in Anchor first, in AnchorConcrete second) {
            switch (first.mode) {
                case AnchorMode.NegativeX:
                    if (second.mode != AnchorMode.PositiveX)
                        return false;
                    break;
                case AnchorMode.NegativeZ:
                    if (second.mode != AnchorMode.PositiveZ)
                        return false;
                    break;
                case AnchorMode.PositiveX:
                    if (second.mode != AnchorMode.NegativeX)
                        return false;
                    break;
                case AnchorMode.PositiveZ:
                    if (second.mode != AnchorMode.NegativeZ)
                        return false;
                    break;
                case AnchorMode.Parent:
                    if (second.mode != AnchorMode.Child)
                        return false;
                    break;
                case AnchorMode.Child:
                    if (second.mode != AnchorMode.Parent)
                        return false;
                    break;
            }
            return first.name == second.name;
        }

        /// <summary>
        /// Defines the behaviour of an anchor when generating a dungeon.
        /// </summary>
        [Serializable]
        public enum AnchorMode {
            /// <summary>
            /// NegativeX anchors only attach to PositiveX anchors. When the maximum room count or depth is reached, these anchors will only generate fallback elements.
            /// </summary>
            NegativeX,
            /// <summary>
            /// NegativeZ anchors only attach to PositiveZ anchors. When the maximum room count or depth is reached, these anchors will only generate fallback elements.
            /// </summary>
            NegativeZ,
            /// <summary>
            /// PositiveX anchors only attach to NegativeX anchors. When the maximum room count or depth is reached, these anchors will only generate fallback elements.
            /// </summary>
            PositiveX,
            /// <summary>
            /// PositiveZ anchors only attach to NegativeZ anchors. When the maximum room count or depth is reached, these anchors will only generate fallback elements.
            /// </summary>
            PositiveZ,
            /// <summary>
            /// Parent anchors only attach to child anchors. These anchors will ignore the maximum room count and depth.
            /// </summary>
            Parent,
            /// <summary>
            /// Child anchors only attach to parent anchors. These anchors will ignore the maximum room count and depth.
            /// </summary>
            Child
        }
        [Serializable]
        public struct Anchor {
            /// <summary>
            /// Anchors only attach to anchors with the same name.
            /// </summary>
            [Tooltip("Anchors only attach to anchors with the same name.")]
            public string name;
            /// <summary>
            /// Anchors only attach to anchors with a compatible mode.
            /// </summary>
            [Tooltip("Anchors only attach to anchors with a compatible mode.")]
            public AnchorMode mode;
            /// <summary>
            /// Offsets the anchor relative to the prefab origin.
            /// </summary>
            [Tooltip("Offsets the anchor relative to the prefab origin.")]
            public Vector2 offset;
            /// <summary>
            /// Provides the elements that can be generated from this anchor point.
            /// </summary>
            [Tooltip("Provides the elements that can be generated from this anchor point.")]
            public DungeonLayoutElementProvider pool;
            /// <summary>
            /// Provides the fallback elements that can be generated from this anchor point.
            /// </summary>
            [Tooltip("Provides the fallback elements that can be generated from this anchor point.")]
            public DungeonLayoutElement fallback;
        }
        [Serializable]
        public struct AnchorConcrete {
            /// <summary>
            /// Anchors only attach to anchors with the same name.
            /// </summary>
            [Tooltip("Anchors only attach to anchors with the same name.")]
            public string name;
            /// <summary>
            /// Anchors only attach to anchors with a compatible mode.
            /// </summary>
            [Tooltip("Anchors only attach to anchors with a compatible mode.")]
            public AnchorMode mode;
            /// <summary>
            /// Offsets the anchor relative to the prefab origin.
            /// </summary>
            [Tooltip("Offsets the anchor relative to the prefab origin.")]
            public Vector2 offset;
            /// <summary>
            /// Provides the elements that can be generated from this anchor point.
            /// </summary>
            [Tooltip("Provides the elements that can be generated from this anchor point.")]
            public DungeonLayoutElementProvider pool;
            /// <summary>
            /// Provides the fallback elements that can be generated from this anchor point.
            /// </summary>
            [Tooltip("Provides the fallback elements that can be generated from this anchor point.")]
            public DungeonLayoutElement fallback;
            /// <summary>
            /// Ignore this variable. This is only used by the dungeon generator to efficiently cull the anchor that was used to generate the current room.
            /// </summary>
            public int index;
            /// <summary>
            /// Ignore this variable. This is only used by the dungeon generator to track the generation depth of anchors in the dungeon because the generation algorithm is not recursive.
            /// </summary>
            public int depth;
        }

        [SerializeField]
        private DungeonLayoutElementProvider m_Root;
        [SerializeField]
        private int m_MaximumRooms;
        [SerializeField]
        private int m_MaximumDepth;
    }
}
