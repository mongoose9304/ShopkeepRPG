using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The types of room a room can be
/// </summary>
public enum RoomType
{
    Combat, Gambling,Shop,EliteCombat,Navigation,Loot,CursedLoot
};
/// <summary>
/// The mostly abstract class that all the rooms are based on
/// </summary>
public class BasicRoom : MonoBehaviour
{
    [Tooltip("The dungeon that I am a part of")]
    public BasicDungeon myDungeon;
    [Tooltip("The type of room I am categorized as")]
    public RoomType myType;
    [Tooltip("Will this room lock when we enter")]
    [SerializeField] protected bool willLockOnEnter;
    [Tooltip("Any specific enemies that we want to spawn in this room")]
    //Currently Unused =(
    [SerializeField] public List<BasicEnemy> specialEnemies = new List<BasicEnemy>();
    [Tooltip("Event that plays when the player enters this room")]
    [SerializeField] protected UnityEvent onEnter;
    [Tooltip("Event that plays when the player exits this room")]
    [SerializeField] protected UnityEvent onExit;
    [Tooltip("REFERENCE to the objects that should be enabled or disabled when locking the room")]
    [SerializeField] protected GameObject[] lockObjects;
    public int size; //0=small,1=medium,2=large


    public virtual void StartRoomActivity()
    {

    }
    public virtual void ChangeSinType(SinType sin_)
    {

    }
    public void SetDungeon(BasicDungeon d_)
    {
        myDungeon = d_;
    }

}
