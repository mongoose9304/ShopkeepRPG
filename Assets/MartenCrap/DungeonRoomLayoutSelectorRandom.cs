using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon/RoomRandom")]
public class DungeonRoomLayoutSelectorRandom : DungeonRoomLayoutSelector {
    [SerializeField]
    private Element[] m_Rooms;

    public override DungeonRoomLayout GetDungeonRoom(DungeonGeneratorContext context) {
        int maximum = 0;
        for (int i = 0; i < m_Rooms.Length; ++i)
            maximum += m_Rooms[i].Value;
        int random = Random.Range(0, maximum);
        int running = 0;
        for (int i = 0; i < m_Rooms.Length; ++i) {
            running += m_Rooms[i].Value;
            if (random < running) {
                if (m_Rooms[i].Key)
                    return m_Rooms[i].Key.GetDungeonRoom(context);
                else
                    return null;
            }
        }
        if (m_Rooms[m_Rooms.Length - 1].Key)
            return m_Rooms[m_Rooms.Length - 1].Key.GetDungeonRoom(context);
        else
            return null;
    }

    [System.Serializable]
    struct Element {
        public DungeonRoomLayoutSelector Key;
        public int Value;
    }
}
