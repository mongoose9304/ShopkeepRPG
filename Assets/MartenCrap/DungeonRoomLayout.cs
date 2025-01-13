using System;
using UnityEngine;

[CreateAssetMenu]
public class DungeonRoomLayout : ScriptableObject {
    public ReadOnlySpan<RectInt> Area {
        get => m_Area;
    }
    public ReadOnlySpan<Door> Doors {
        get => m_Doors;
    }

    [Serializable]
    public struct Door {
        public readonly string Name {
            get => m_Name;
        }
        public readonly Vector2Int Position {
            get => m_Position;
        }
        public readonly DoorDirection Direction {
            get => m_Direction;
        }

        [SerializeField]
        private string m_Name;
        [SerializeField]
        private Vector2Int m_Position;
        [SerializeField]
        private DoorDirection m_Direction;
    }
    [Serializable]
    public enum DoorDirection {
        xPositive,
        zPositive,
        xNegative,
        zNegative
    }

    [SerializeField]
    private RectInt[] m_Area;
    [SerializeField]
    private Door[] m_Doors;
}
