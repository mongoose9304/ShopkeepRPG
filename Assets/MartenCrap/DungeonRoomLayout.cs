using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon/Room")]
public sealed class DungeonRoomLayout : DungeonRoomLayoutSelector {
    /// <summary>
    /// Defines the area that the room consumes; The dungeon generator will not generate overlapping rooms.
    /// </summary>
    public ReadOnlySpan<Rect> Area {
        get => m_Area;
    }
    /// <summary>
    /// Defines the entrances and exits for the room; The dungeon generator will attach rooms to doors.
    /// </summary>
    public ReadOnlySpan<Door> Doors {
        get => m_Doors;
    }
    /// <summary>
    /// Defines the prefab that will be placed into the scene when the room is placed in the dungeon.
    /// </summary>
    public GameObject Prefab {
        get => m_Prefab;
    }
    
    public override DungeonRoomLayout GetDungeonRoom(DungeonGeneratorContext context) {
        return this;
    }
    public static bool DoorDirectionsCanConnect(DoorDirection a, DoorDirection b) {
        if (a == DoorDirection.decorative && b == DoorDirection.decorative)
            return true;
        if (a == DoorDirection.xNegative && b == DoorDirection.xPositive)
            return true;
        if (a == DoorDirection.xPositive && b == DoorDirection.xNegative)
            return true;
        if (a == DoorDirection.zNegative && b == DoorDirection.zPositive)
            return true;
        if (a == DoorDirection.zPositive && b == DoorDirection.zNegative)
            return true;
        return false;
    }

    [Serializable]
    public struct Door {
        /// <summary>
        /// The name of the door; Doors only attach to doors with the same name.
        /// </summary>
        public readonly string Name {
            get => m_Name;
        }
        /// <summary>
        /// The position of the door; Rooms are attached at doorways.
        /// </summary>
        public Vector2 Position {
            readonly get => m_Position;
            set => m_Position = value;
        }
        /// <summary>
        /// The direction of the door; Rooms only attach to doors with opposing directions. For more details see <see cref="DoorDirection"/>.
        /// </summary>
        public DoorDirection Direction {
            readonly get => m_Direction;
            set => m_Direction = value;
        }
        /// <summary>
        /// The rooms that can be spawned from this door; This is used by the dungeon generator to decide which room will be attached to this door.
        /// </summary>
        public readonly DungeonRoomLayoutSelector Adjacent {
            get => m_Adjacent;
        }
        /// <summary>
        /// The rooms that can be spawned from this door when the adjacent room is invalid; This is used by the dungeon generator to decide which room will be attached to this door.
        /// </summary>
        public readonly DungeonRoomLayoutSelector AdjacentFallback {
            get => m_AdjacentFallback;
        }

        [SerializeField]
        private string m_Name;
        [SerializeField]
        private Vector2 m_Position;
        [SerializeField]
        private DoorDirection m_Direction;
        [SerializeField]
        private DungeonRoomLayoutSelector m_Adjacent;
        [SerializeField]
        private DungeonRoomLayoutSelector m_AdjacentFallback;
    }
    [Serializable]
    public enum DoorDirection {
        /// <summary>
        /// Doors with the x positive direction only attach to doors with the x negative direction.
        /// </summary>
        xPositive,
        /// <summary>
        /// Doors with the z positive direction only attach to doors with the z negative direction.
        /// </summary>
        zPositive,
        /// <summary>
        /// Doors with the x negative direction only attach to doors with the x positive direction.
        /// </summary>
        xNegative,
        /// <summary>
        /// Doors with the z negative direction only attach to doors with the z positive direction.
        /// </summary>
        zNegative,
        /// <summary>
        /// Decorative doors only attach to other decorative doors; Room collision is also ignored. Decorative doors are primarily used for placing prefabs inside rooms.
        /// </summary>
        decorative,
    }

    [SerializeField]
    private Rect[] m_Area;
    [SerializeField]
    private Door[] m_Doors;
    [SerializeField]
    private GameObject m_Prefab;//TODO: Convert into string containing asset path; This will prevent prefabs from being pre-emptively loaded;
}
