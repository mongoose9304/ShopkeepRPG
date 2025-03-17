using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class DungeonEncounter : MonoBehaviour
{
    [SerializeField]
    public List<DungeonEncounterPhase> phases;
    public List<DungeonEncounterDoor> dungeonDoors;

    private int playerCount = 0;
    private int phaseCounter;

    // Start is called before the first frame update
    void Start()
    {
        phaseCounter = 0;// phases[0].GetEnemyCount();
        //phases[0].Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncrementPlayers()
    {
        playerCount += 1;
        if (playerCount == 1) // TODO: Make this either 1 or 2 depending on how many players are in
        {
            Activate();
        }
    }

    public void DecrementPlayers()
    {
        playerCount -= 1;
    }

    public void Activate()
    {
        Debug.Log("Encounter starting");
        if (phases.Count > 0)
        {
            phases[0].Activate();
            phaseCounter = phases[0].GetEnemyCount();
        }
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
                phases[0].Activate();
                phaseCounter = phases[0].GetEnemyCount();
                // Activate the next phase
            }
        }
    }
}
