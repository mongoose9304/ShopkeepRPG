using UnityEngine;

namespace Dungeons {
    /// <summary>
    /// Provides possible elements to the dungeon generator.
    /// </summary>
    public abstract class DungeonLayoutElementProvider : ScriptableObject {
        /// <summary>
        /// Responsible for registering elements that could potentially attach to the anchor.
        /// </summary>
        /// <param name="dungeon">The dungeon.</param>
        /// <param name="anchor">The anchor.</param>
        /// <param name="weight">The weight multiplier.</param>
        public abstract void ProvideLayoutElements(DungeonGenerator dungeon, in DungeonLayout.AnchorConcrete anchor, float weight);
    }
}
