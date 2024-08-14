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
    public int enemyLevel;
    public int eliteEnemyLevel;
    public List<Transform> regularRoomSpots = new List<Transform>();
    

   public void SetUpEnemies()
    {
        foreach (BasicEnemy enemy in regularEnemies)
        {
            EnemyManager.instance.CreateEnemyItem(enemy.myBaseData.originalName, enemy.gameObject);
        }
        foreach (BasicEnemy enemy in eliteEnemies)
        {
            EnemyManager.instance.CreateEnemyItem(enemy.myBaseData.originalName, enemy.gameObject,true);
        }
    }
}
