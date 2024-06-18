using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum RoomType
{
    Combat, Gambling,Shop,EliteCombat,Navigation,Loot
};
public class BasicRoom : MonoBehaviour
{
    public BasicDungeon myDungeon;
    public RoomType myType;
    [SerializeField] protected bool willLockOnEnter;
    [SerializeField] public List<BasicEnemy> specialEnemies = new List<BasicEnemy>();
    [SerializeField] protected UnityEvent onEnter;
    [SerializeField] protected UnityEvent onExit;
    [SerializeField] protected GameObject lockObject;
    public int size; //0=small,1=medium,2=large


    public virtual void StartRoomActivity()
    {

    }

}
