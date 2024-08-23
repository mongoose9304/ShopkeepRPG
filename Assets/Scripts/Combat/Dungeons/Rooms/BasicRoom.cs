using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum RoomType
{
    Combat, Gambling,Shop,EliteCombat,Navigation,Loot,CursedLoot
};
public class BasicRoom : MonoBehaviour
{
    public BasicDungeon myDungeon;
    public RoomType myType;
    [SerializeField] protected bool willLockOnEnter;
    [SerializeField] public List<BasicEnemy> specialEnemies = new List<BasicEnemy>();
    [SerializeField] protected UnityEvent onEnter;
    [SerializeField] protected UnityEvent onExit;
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
