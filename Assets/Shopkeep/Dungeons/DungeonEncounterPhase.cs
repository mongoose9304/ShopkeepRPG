using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DungeonEncounterPhase : MonoBehaviour
{
    [SerializeField]
    public UnityEvent start;
    [SerializeField]
    public UnityEvent end;
    [SerializeField]
    public List<DungeonEncounterSpawner> enemies;

    public int GetEnemyCount()
    {
        return enemies.Count;
    }

    public void Activate()
    {
        start.Invoke();
        Debug.Log(enemies.Count.ToString());
        for (int i = 0; i < enemies.Count; ++i)
        {
            enemies[i].Spawn();
        }
    }

    public void End()
    {
        end.Invoke();
    }
}
