using UnityEngine;

namespace Dungeons {
    public struct DungeonGeneratorElement {
        public DungeonLayoutElement Element {
            readonly get;
            set;
        }
        public Vector2 Position {
            readonly get;
            set;
        }
    }
}
