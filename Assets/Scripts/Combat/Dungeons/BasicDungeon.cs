using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// All the avilable sins 
/// </summary>
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
/// <summary>
/// The logic for a combat dungeon (collection of rooms) 
/// </summary>
public class BasicDungeon : MonoBehaviour
{
    [Header("Variables for Difficulty")]
    [Tooltip("The types of enemies that can spawn")]
    public List<BasicEnemy> regularEnemies=new List<BasicEnemy>();
    [Tooltip("The types of elite enemies that can spawn")]
    public List<BasicEnemy> eliteEnemies = new List<BasicEnemy>();
    [Tooltip("The level to spawn enemies at, for arenas this comes from the wave list")]
    public int enemyLevel;
    [Tooltip("The level to spawn enemies at, for arenas this comes from the wave list")]
    public int eliteEnemyLevel;
    [Tooltip("The level to spawn traps at")]
    [SerializeField] int trapLevel;
    [Tooltip("The value of treasure chests")]
    [SerializeField] int treasureChestAmount;
    [Tooltip("The min money enemies can drop")]
    [SerializeField] int basicEnemyValueMin;
    [Tooltip("The max money enemies can drop")]
    [SerializeField] int basicEnemyValueMax;
    [Tooltip("The min exp enemies can drop")]
    [SerializeField] int basicEnemyExpValueMin;
    [Tooltip("The max exp enemies can drop")]
    [SerializeField] int basicEnemyExpValueMax;
    [Tooltip("Should we spawn one team or use many?")]
    public bool useRandomTeams;

    [Header("References")]
    [Tooltip("REFERNCE to the rooms in this dungeon")]
    public BasicRoom[] regularRooms;
    [Tooltip("The current Sin used by this dungeon, will be set by Dungeon manager")]
    public SinType mySin;
    [Tooltip("REFERNCE to the location to spawn the player")]
    public Transform playerStart;
    [Tooltip("REFERNCE to the places rooms can be spawned")]
    public List<Transform> regularRoomSpots = new List<Transform>();
    [Tooltip("REFERNCE to the teams that exist in this domain")]
    [SerializeField] public List<string> availableTeams = new List<string>();
    [Tooltip("REFERNCE to the team if there is only one")]
    public string onlyTeam;
    

    private void OnEnable()
    {
        regularRooms = GetComponentsInChildren<BasicRoom>(true);
        foreach(BasicRoom room in regularRooms)
        {
            room.SetDungeon(this);
        }
    }
    /// <summary>
    /// Create the enemy pools so we can just activate them later
    /// </summary>
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
    /// <summary>
    /// Change the enemy managers possible teams to match ours 
    /// </summary>
    public void SetEnemyManagerTeams()
    {
        EnemyManager.instance.randomTeams = availableTeams;
        EnemyManager.instance.singleTeam = onlyTeam;
        EnemyManager.instance.useRandomTeams = useRandomTeams;
    }
}
