using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SinType
{
    Greed,
    Lust,
    Pride,
    Vainglory,
    Sloth,
    Gluttony,
    Envy,
    Capriciousness,
    Wrath

}

public class BasicDungeon : MonoBehaviour
{
    public List<BasicEnemy> regularEnemies=new List<BasicEnemy>();
    public List<BasicEnemy> eliteEnemies = new List<BasicEnemy>();
    public List<BasicRoom> regularRooms = new List<BasicRoom>();
    public List<BasicRoom> smallRooms = new List<BasicRoom>();
    public List<BasicRoom> bigRooms = new List<BasicRoom>();
    public SinType mySin;
    public List<Transform> regularRoomSpots = new List<Transform>();
    public void Start()
    {
        foreach (BasicEnemy enemy in regularEnemies)
        {
            EnemyManager.instance.CreateEnemyItem(enemy.myBaseData.originalName, enemy.gameObject);
        }
        /* foreach(Transform transform in regularRoomSpots)
         {
           GameObject obj=  Instantiate(regularRooms[Random.Range(0,regularRooms.Count)].gameObject,transform.position,transform.rotation);
             obj.GetComponent<BasicRoom>().myDungeon = this;
            // transform.gameObject.SetActive(false);
            foreach(BasicEnemy enemy in regularEnemies)
             {
                 EnemyManager.instance.CreateEnemyItem(enemy.myBaseData.originalName, enemy.gameObject);
             }

         }
        */
    }

    public void CreateSpecificRoom(int roomSize,Transform roomLocation,RoomType desiredType)
    {
     /*   List<BasicRoom> tempRooms = new List<BasicRoom>();
        foreach(BasicRoom room_ in smallRooms)
        {
            if (room_.myType == desiredType)
                tempRooms.Add(room_);
        }
        foreach (BasicRoom room_ in regularRooms)
        {
            if (room_.myType == desiredType)
                tempRooms.Add(room_);

        }
        foreach (BasicRoom room_ in bigRooms)
        {
            if (room_.myType == desiredType)
                tempRooms.Add(room_);
        }
     */
    }
}
