using UnityEngine;

public class DungeonRoom : MonoBehaviour {
    private void OnDrawGizmos() {
        if (m_Layout) {
            Random.InitState(m_Layout.GetInstanceID());
            Gizmos.color = Random.ColorHSV(0.0f, 1.0f, 1.0f, 1.0f, 0.7f, 1.0f);
            foreach (ref readonly var rect in m_Layout.Area) {
                Vector3 size = new(rect.size.x, 3.0f, rect.size.y);
                Vector3 position = new(rect.position.x, 0.0f, rect.position.y);
                Gizmos.DrawWireCube(transform.position + position + size * 0.5f, size);
            }
            foreach (ref readonly var door in m_Layout.Doors) {
                Vector3 point = new(door.Position.x, 0.0f, door.Position.y);
                Vector3 direction = new();
                switch (door.Direction) {
                    case DungeonRoomLayout.DoorDirection.xPositive:
                        direction = new(1, 0, 0);
                        break;
                    case DungeonRoomLayout.DoorDirection.zPositive:
                        direction = new(0, 0, 1);
                        break;
                    case DungeonRoomLayout.DoorDirection.xNegative:
                        direction = new(-1, 0, 0);
                        break;
                    case DungeonRoomLayout.DoorDirection.zNegative:
                        direction = new(0, 0, -1);
                        break;
                }
                Gizmos.DrawLine(transform.position + point, transform.position + point + direction);
                Gizmos.DrawSphere(transform.position + point, 0.2f);
            }
        }
    }

    [SerializeField]
    private DungeonRoomLayout m_Layout;
}
