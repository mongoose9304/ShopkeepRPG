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
    public BasicRoom[] regularRooms;
    public SinType mySin;
    public Transform playerStart;
    public int enemyLevel;
    public int eliteEnemyLevel;
    [SerializeField] int trapLevel;
    [SerializeField] int treasureChestAmount;
    [SerializeField] int basicEnemyValueMin;
    [SerializeField] int basicEnemyValueMax;
    [SerializeField] int basicEnemyExpValueMin;
    [SerializeField] int basicEnemyExpValueMax;
    public List<Transform> regularRoomSpots = new List<Transform>();

    private void OnEnable()
    {
        regularRooms = GetComponentsInChildren<BasicRoom>(true);
        foreach(BasicRoom room in regularRooms)
        {
            room.SetDungeon(this);
        }
    }
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

    public int GetTrapLevel()
    {
        return trapLevel;
    }
    public int GetTreasureChestAmount()
    {
        return treasureChestAmount;
    }
    public int GetBasicEnemyValue()
    {
        return Random.Range(basicEnemyValueMin, basicEnemyValueMax);
    }
    public int GetBasicEnemyExpValue()
    {
        return Random.Range(basicEnemyExpValueMin, basicEnemyExpValueMax);
    }
    public void ChangeSin(SinType sin_)
    {
        foreach(BasicRoom room in regularRooms)
        {
            room.ChangeSinType(sin_);
        }
    }
}
