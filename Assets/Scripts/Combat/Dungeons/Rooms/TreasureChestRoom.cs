using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChestRoom : BasicRoom
{
    public override void StartRoomActivity()
    {
        if(myType==RoomType.CursedLoot)
        {
            //curse player, should be from list of curses in dungeon
        }
    }
}
