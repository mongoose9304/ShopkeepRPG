using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public BasicDungeon currentDungeon;

    public static DungeonManager instance;

    private void Awake()
    {
        instance = this;
        
    }
    private void Start()
    {
        ChangeLevel(currentDungeon);
    }

    public int GetEnemyLevel() { return currentDungeon.enemyLevel; }
    public int GetEliteEnemyLevel() { return currentDungeon.eliteEnemyLevel; }


    public void ChangeLevel(BasicDungeon dungeon_)
    {
        currentDungeon = dungeon_;
        currentDungeon.SetUpEnemies();
    }
}
