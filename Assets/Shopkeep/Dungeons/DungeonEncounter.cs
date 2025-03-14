using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DungeonEncounterPhase {
    [SerializeField]
    public UnityEvent start;
    [SerializeField]
    public UnityEvent end;
    public List<DungeonEncounterSpawner> enemies = new List<DungeonEncounterSpawner>();

    public int GetEnemyCount()
    {
        return enemies.Count;
    }

    public void Start()
    {
        start.Invoke();
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


public class DungeonEncounter : MonoBehaviour
{
    public List<DungeonEncounterPhase> phases;
    public List<DungeonEncounterDoor> dungeonDoors;

    private int phaseCounter;

    // Start is called before the first frame update
    void Start()
    {
        phaseCounter = phases[0].GetEnemyCount();
        phases[0].Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {

    }

    public void DecrementPhase()
    {
        phaseCounter--;
        if (phaseCounter <= 0)
        {
            phases[0].End();
            phases.RemoveAt(0);
            if (phases.Count > 0)
            {
                phases[0].Start();
                phaseCounter = phases[0].GetEnemyCount();
                // Activate the next phase
            }
        }
    }
}
