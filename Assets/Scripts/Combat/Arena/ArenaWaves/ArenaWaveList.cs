using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The amount and intensity of the enemy waves of an arena 
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ArenaWaveList", order = 1)]
public class ArenaWaveList : ScriptableObject
{
    public int maxWaves;
    public int startingEnemies;
    public int extraEnemiesPerWave;
    public int startingEnemyLevel;
    public int extraEnemyLevelPerWave;


    
}
