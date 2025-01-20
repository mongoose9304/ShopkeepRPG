using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class DungeonGeneratorContext {
    public IEnumerable<Rect> Bounds {
        get => m_Bounds;
    }

    public IEnumerator Generate(DungeonRoomLayoutSelector root, Transform rootTransform) {
        var rootRoom = root.GetDungeonRoom(this);
        //paste root room.
        UnityEngine.Object.Instantiate(rootRoom.Prefab
            , rootTransform.position
            , Quaternion.identity
            , rootTransform);
        yield return new WaitForSeconds(1.0f / 20.0f);
        //include root room doors.
        for (int i = 0; i < rootRoom.Doors.Length; ++i)
            PushDoor(rootRoom.Doors[i]);
        //include root room collision.
        for (int i = 0; i < rootRoom.Area.Length; ++i)
            m_Bounds.Add(rootRoom.Area[i]);
        while (TryPopDoor(out var door)) {
            //select and validate room for door.
            if (GetAdjacentRoomAndDoor(door, out var room, out var doorIndex)) {
                var pasteDoor = room.Doors[doorIndex];
                var pastePosition = door.Position - pasteDoor.Position;
                //paste room.
                UnityEngine.Object.Instantiate(room.Prefab
                    , rootTransform.position + new Vector3(pastePosition.x, 0.0f, pastePosition.y)
                    , Quaternion.identity
                    , rootTransform);
                //include pasted room doors.
                for (int i = 0; i < room.Doors.Length; ++i) {
                    if (i != doorIndex) {
                        var current = room.Doors[i];
                        current.Position += pastePosition;
                        PushDoor(current);
                    }
                }
                //include pasted room collision.
                for (int i = 0; i < room.Area.Length; ++i) {
                    var area = room.Area[i];
                    area.position += pastePosition;
                    m_Bounds.Add(area);
                }
                yield return new WaitForSeconds(1.0f / 20.0f);
            }
        }
    }
    
    public void PushDoor(in DungeonRoomLayout.Door door) {
        m_Doors.Push(door);
    }
    public bool TryPopDoor(out DungeonRoomLayout.Door door) {
        return m_Doors.TryPop(out door);
    }
    public bool GetAdjacentRoomAndDoor(in DungeonRoomLayout.Door door, out DungeonRoomLayout adjacent, out int adjacentDoor) {
        //Select and validate adjacent room.
        if (door.Adjacent) {
            var adj = door.Adjacent.GetDungeonRoom(this);
            if (adj) {
                //select any valid door.
                var adjDoors = adj.Doors;
                for (int i = 0; i < adjDoors.Length; ++i) {
                    ref readonly var adjDoor = ref adjDoors[i];
                    //validate door direction.
                    if (!DungeonRoomLayout.DoorDirectionsCanConnect(door.Direction, adjDoor.Direction)) {
                        continue;//Door directions do not match.
                    }
                    //validate door name.
                    if (door.Name != adjDoor.Name) {
                        continue;//Door names do not match.
                    }
                    //validate door will not cause intersection with current dungeon.
                    if (Intersects(adj.Area, door.Position - adjDoor.Position)) {
                        continue;//Door would cause intersection.
                    }
                    adjacent = adj;
                    adjacentDoor = i;
                    return true;
                }
            }
        }
        //Select and return fallback room.
        if (door.AdjacentFallback) {
            var adj = door.AdjacentFallback.GetDungeonRoom(this);
            if (adj) {
                //select any valid door.
                var adjDoors = adj.Doors;
                for (int i = 0; i < adjDoors.Length; ++i) {
                    ref readonly var adjDoor = ref adjDoors[i];
                    //validate door direction.
                    if (!DungeonRoomLayout.DoorDirectionsCanConnect(door.Direction, adjDoor.Direction)) {
                        continue;//Door directions do not match.
                    }
                    //validate door name.
                    if (door.Name != adjDoor.Name) {
                        continue;//Door names do not match.
                    }
                    //validate door will not cause intersection with current dungeon.
                    if (Intersects(adj.Area, door.Position - adjDoor.Position)) {
                        continue;//Door would cause intersection.
                    }
                    adjacent = adj;
                    adjacentDoor = i;
                    return true;
                }
            }
        }
        //Return nothing because there is no valid room or fallback room.
        adjacent = null;
        adjacentDoor = 0;
        return false;
    }

    public bool Intersects(in Rect bounds) {
        for (int i = 0; i < m_Bounds.Count; ++i)
            if (m_Bounds[i].Overlaps(bounds))
                return true;
        return false;
    }
    public bool Intersects(in ReadOnlySpan<Rect> bounds) {
        for (int i = 0; i < bounds.Length; ++i)
            if (Intersects(bounds[i]))
                return true;
        return false;
    }
    public bool Intersects(in ReadOnlySpan<Rect> bounds, Vector2 boundsOffset) {
        for (int i = 0; i < bounds.Length; ++i) {
            Rect area = bounds[i];
            area.position += boundsOffset;
            if (Intersects(area))
                return true;
        }
        return false;
    }
    
    private readonly List<Rect> m_Bounds = new();
    private readonly Dictionary<string, int> m_Counters = new();
    private readonly Stack<DungeonRoomLayout.Door> m_Doors = new();
}
