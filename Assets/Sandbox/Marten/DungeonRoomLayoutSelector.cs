using UnityEngine;

/// <summary>
/// Defines the algorithm used by the dungeon generator when selecting the next room to generate.
/// </summary>
public abstract class DungeonRoomLayoutSelector : ScriptableObject {
    /// <summary>
    /// Called by the dungeon generator when selecting the next room to generate.
    /// </summary>
    /// <param name="context">The dungeon generator context.</param>
    /// <returns>The next room to generate; Note that, if the room would overlap the existing dungeon, lacks the proper door, or is otherwise invalid, a new room will be selected; After multiple failed attempts, dungeon generation will restart entirely.</returns>
    public abstract DungeonRoomLayout GetDungeonRoom(DungeonGeneratorContext context);
}
